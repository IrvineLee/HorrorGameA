using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;

using Helper;
using Newtonsoft.Json;
using Personal.Entity;
using Personal.Data;
using Personal.Localization;

namespace Personal.CustomEdit
{
	public enum DataEnumType
	{
		[StringValue("")] Window = 0,                   // Later(take out put it into it's own script)
		[StringValue("AudioBGMType")] BGM,
		[StringValue("AudioSFXType")] SFX,
		[StringValue("")] Item,                         // Later(string naming with prefab addressable)
		[StringValue("QuestType")] Quest,
		[StringValue("AchievementType")] Achievement,
		[StringValue("ReadFileType")] ReadFile,
		[StringValue("KeyEventType")] KeyEvent,
	}

	[Serializable]
	public class ExcelToEnumGroup
	{
		public List<ExcelToEnum> excelToEnumList = new();

		public ExcelToEnumGroup(List<ExcelToEnum> excelToEnumList) { this.excelToEnumList = excelToEnumList; }
	}

	[Serializable]
	public class ExcelToEnum
	{
		[HideInInspector] public string path;
		public Object definition;
		public DataEnumType dataEnumType;

		public ExcelToEnum(string path, DataEnumType dataEnumType)
		{
			this.path = path;
			this.dataEnumType = dataEnumType;
		}
	}

	public class EnumLine
	{
		public string enumName;
		public bool isNextLine;

		public EnumLine(string enumName, bool isNextLine)
		{
			this.enumName = enumName;
			this.isNextLine = isNextLine;
		}
	}

	public class ExcelToEnumEditor : EditorWindow
	{
		[SerializeField] List<ExcelToEnum> excelToEnumList = new();

		Editor editor;

		[MenuItem("Window/ExcelToEnum")]
		static void Init() { GetWindow<ExcelToEnumEditor>("ExcelToEnum", true); }

		void OnEnable()
		{
			if (!editor) { editor = Editor.CreateEditor(this); }

			(editor as ListTestEditorDrawer)?.DeserializeData();
		}

		void OnGUI()
		{
			editor.OnInspectorGUI();
		}

		void OnInspectorUpdate() { Repaint(); }

		void OnDisable()
		{
			(editor as ListTestEditorDrawer)?.SerializeData();
		}
	}

	[CustomEditor(typeof(ExcelToEnumEditor), true)]
	public class ListTestEditorDrawer : Editor
	{
		SerializedProperty excelToEnumSP;

		public override void OnInspectorGUI()
		{
			if (excelToEnumSP == null) return;

			excelToEnumSP = serializedObject.FindProperty("excelToEnumList");
			EditorGUILayout.PropertyField(excelToEnumSP, new GUIContent("Excel To Enum List"), true);

			if (GUILayout.Button("Convert To Enum")) ConvertToEnum();
		}

		public void SerializeData()
		{
			List<ExcelToEnum> excelToEnumList = new();

			int count = excelToEnumSP.arraySize;
			for (int i = 0; i < count; i++)
			{
				var objectRef = excelToEnumSP.GetArrayElementAtIndex(i).FindPropertyRelative("definition").objectReferenceValue;
				var enumType = excelToEnumSP.GetArrayElementAtIndex(i).FindPropertyRelative("dataEnumType").intValue;
				var path = AssetDatabase.GetAssetPath(objectRef);

				ExcelToEnum excelToEnum = new ExcelToEnum(path, (DataEnumType)enumType);
				excelToEnumList.Add(excelToEnum);
			}

			ExcelToEnumGroup excelToEnumGroup = new ExcelToEnumGroup(excelToEnumList);

			var str = JsonConvert.SerializeObject(excelToEnumGroup);
			EditorPrefs.SetString("excelToEnumGroup", str);
		}

		public void DeserializeData()
		{
			if (EditorPrefs.HasKey("excelToEnumGroup"))
			{
				var str = EditorPrefs.GetString("excelToEnumGroup");
				var temp = JsonConvert.DeserializeObject<ExcelToEnumGroup>(str);

				for (int i = 0; i < temp.excelToEnumList.Count; i++)
				{
					var excelToEnum = temp.excelToEnumList[i];
					var deserializedExcelObject = AssetDatabase.LoadAssetAtPath(excelToEnum.path, typeof(Object));
					var deserializedEnumType = excelToEnum.dataEnumType;

					excelToEnumSP = serializedObject.FindProperty("excelToEnumList");
					excelToEnumSP.arraySize++;

					var currentSP = excelToEnumSP.GetArrayElementAtIndex(i);
					var excelObjectSP = currentSP.FindPropertyRelative("definition");
					var dataEnumTypeSP = currentSP.FindPropertyRelative("dataEnumType");

					excelObjectSP.objectReferenceValue = deserializedExcelObject;
					dataEnumTypeSP.intValue = (int)deserializedEnumType;
				}
				return;
			}

			excelToEnumSP = serializedObject.FindProperty("excelToEnumList");
		}

		void ConvertToEnum()
		{
			for (int i = 0; i < excelToEnumSP.arraySize; i++)
			{
				var currentSP = excelToEnumSP.GetArrayElementAtIndex(i);
				var excelObjectSP = currentSP.FindPropertyRelative("definition");
				var dataEnumTypeSP = currentSP.FindPropertyRelative("dataEnumType");

				switch ((DataEnumType)dataEnumTypeSP.intValue)
				{
					case DataEnumType.Achievement: HandleEnumGeneration<MasterAchievement, AchievementEntity>(excelObjectSP, "Personal.Achievement", "AchievementType"); break;
					case DataEnumType.BGM: HandleEnumGeneration<MasterBGM, BGMEntity>(excelObjectSP, "Personal.Setting.Audio", "AudioBGMType"); break;
					case DataEnumType.SFX: HandleEnumGeneration<MasterSFX, SFXEntity>(excelObjectSP, "Personal.Setting.Audio", "AudioSFXType"); break;
					case DataEnumType.ReadFile: HandleEnumGeneration<MasterReadFile, ReadFileEntity>(excelObjectSP, "Personal.UI", "ReadFileType"); break;
				}
			}

			Debug.Log("Completed!");
		}

		void HandleEnumGeneration<T1, T2>(SerializedProperty serializedProperty, string namespaceName, string enumTypeStr) where T1 : MasterGeneric<T2, int> where T2 : GenericNameEntity
		{
			T1 currentMasterData = (T1)serializedProperty.objectReferenceValue;

			var scriptArray = AssetDatabase.FindAssets(enumTypeStr);
			var path = "";

			foreach (var script in scriptArray)
			{
				var currentPath = AssetDatabase.GUIDToAssetPath(script);
				var currentObject = AssetDatabase.LoadAssetAtPath(currentPath, typeof(Object));

				if (!currentObject.name.Equals(enumTypeStr)) continue;

				path = currentPath;
				break;
			}

			var dictionary = currentMasterData.Dictionary;
			GenerateEnumScript(GetEnumLineList(dictionary), path, namespaceName, enumTypeStr);
		}

		List<EnumLine> GetEnumLineList<T>(IReadOnlyDictionary<int, T> dictionary) where T : GenericNameEntity
		{
			List<EnumLine> enumLineList = new();

			int previousID = -1;
			bool isNextLine = false;

			foreach (var entity in dictionary)
			{
				string enumName = entity.Value.name + " = " + entity.Key.ToString() + ",";

				if (previousID != -1 && (previousID + 1) != entity.Key) isNextLine = true;
				previousID = entity.Key;

				enumLineList.Add(new EnumLine(enumName, isNextLine));
			}

			return enumLineList;
		}

		void GenerateEnumScript(List<EnumLine> enumLineList, string path, string namespaceName, string enumName)
		{
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				streamWriter.WriteLine("");

				streamWriter.WriteLine("namespace " + namespaceName);
				streamWriter.WriteLine("{");
				streamWriter.WriteLine("\tpublic enum " + enumName);
				streamWriter.WriteLine("\t{");

				for (int i = 0; i < enumLineList.Count; i++)
				{
					EnumLine enumLine = enumLineList[i];
					string name = enumLine.enumName;
					bool isNextLine = enumLine.isNextLine;

					//streamWriter.WriteLine("\t\t[StringValue(\"" + name + "\")]");
					if (isNextLine) streamWriter.WriteLine("");
					streamWriter.WriteLine("\t\t" + name);
				}

				streamWriter.WriteLine("\t}");
				streamWriter.WriteLine("}");
			}
			AssetDatabase.Refresh();
		}
	}
}