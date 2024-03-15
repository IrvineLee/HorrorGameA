using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;

using Newtonsoft.Json;
using Personal.Entity;
using Personal.Data;
using Personal.Localization;

namespace Personal.CustomEdit
{
	public enum DataEnumType
	{
		Window = 0,
		BGM,
		SFX,
		Item,
		Quest,
		Achievement,
		ReadFile,
		KeyEvent,
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
		public string assetAddress;
		public bool isNextLine;

		public EnumLine(string enumName, string assetAddress, bool isNextLine)
		{
			this.enumName = enumName;
			this.assetAddress = assetAddress;
			this.isNextLine = isNextLine;
		}
	}

	public class ExcelToEnumEditor : EditorWindow
	{
		[SerializeField] List<ExcelToEnum> excelToEnumList = new();

		Editor editor;

		[MenuItem("Tools/Excel To Enum")]
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
			if (GUILayout.Button("Save definition")) SerializeData();
			if (GUILayout.Button("Load definition")) DeserializeData();
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

			Debug.Log("Saved!");
		}

		public void DeserializeData()
		{
			Debug.Log("Loaded!");

			if (EditorPrefs.HasKey("excelToEnumGroup"))
			{
				var str = EditorPrefs.GetString("excelToEnumGroup");
				var temp = JsonConvert.DeserializeObject<ExcelToEnumGroup>(str);

				excelToEnumSP?.ClearArray();
				excelToEnumSP = serializedObject.FindProperty("excelToEnumList");

				for (int i = 0; i < temp.excelToEnumList.Count; i++)
				{
					var excelToEnum = temp.excelToEnumList[i];
					var deserializedExcelObject = AssetDatabase.LoadAssetAtPath(excelToEnum.path, typeof(Object));
					var deserializedEnumType = excelToEnum.dataEnumType;

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
					case DataEnumType.Window: HandleEnumGeneration<MasterWindowUI, WindowUIEntity>(excelObjectSP, "Personal.UI.Window", "WindowUIType", "The reason a window appears."); break;
					case DataEnumType.BGM: HandleEnumGeneration<MasterBGM, BGMEntity>(excelObjectSP, "Personal.Setting.Audio", "AudioBGMType"); break;
					case DataEnumType.SFX: HandleEnumGeneration<MasterSFX, SFXEntity>(excelObjectSP, "Personal.Setting.Audio", "AudioSFXType"); break;
					case DataEnumType.Item: HandleEnumGeneration<MasterItem, ItemEntity>(excelObjectSP, "Personal.Item", "ItemType", "", true); break;
					case DataEnumType.Quest: HandleEnumGeneration<MasterQuest, QuestEntity>(excelObjectSP, "Personal.Quest", "QuestType"); break;
					case DataEnumType.Achievement: HandleEnumGeneration<MasterAchievement, AchievementEntity>(excelObjectSP, "Personal.Achievement", "AchievementType"); break;
					case DataEnumType.ReadFile: HandleEnumGeneration<MasterReadFile, ReadFileEntity>(excelObjectSP, "Personal.UI", "ReadFileType"); break;
					case DataEnumType.KeyEvent: HandleEnumGeneration<MasterKeyEvent, KeyEventEntity>(excelObjectSP, "Personal.KeyEvent", "KeyEventType"); break;
				}
			}

			Debug.Log("Completed!");
		}

		void HandleEnumGeneration<T1, T2>(SerializedProperty serializedProperty, string namespaceName, string enumTypeStr, string comments = "", bool isAddAssetAdress = false)
			where T1 : MasterGeneric<T2, int> where T2 : GenericNameEntity
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
			GenerateEnumScript(GetEnumLineList(dictionary), path, namespaceName, enumTypeStr, comments);
		}

		List<EnumLine> GetEnumLineList<T>(IReadOnlyDictionary<int, T> dictionary) where T : GenericNameEntity
		{
			List<EnumLine> enumLineList = new();
			int previousID = -1;

			foreach (var entity in dictionary)
			{
				bool isNextLine = false;

				string enumName = entity.Value.name + " = " + entity.Key.ToString() + ",";
				string assetAddress = string.IsNullOrEmpty(entity.Value.assetAddress) ? "" : "[StringValue(" + entity.Value.assetAddress + ")]";

				if (previousID != -1 && (previousID + 1) != entity.Key) isNextLine = true;
				previousID = entity.Key;

				enumLineList.Add(new EnumLine(enumName, assetAddress, isNextLine));
			}

			return enumLineList;
		}

		void GenerateEnumScript(List<EnumLine> enumLineList, string path, string namespaceName, string enumName, string comments)
		{
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				if (enumLineList.Where((x => !string.IsNullOrEmpty(x.assetAddress))).Any())
				{
					streamWriter.WriteLine("using Helper;");
				}

				streamWriter.WriteLine("");

				streamWriter.WriteLine("namespace " + namespaceName);
				streamWriter.WriteLine("{");

				// Add comments.
				if (!string.IsNullOrEmpty(comments))
				{
					streamWriter.WriteLine("\t/// <summary>");
					streamWriter.WriteLine("\t/// " + comments);
					streamWriter.WriteLine("\t/// <summary>");
				}

				streamWriter.WriteLine("\tpublic enum " + enumName);
				streamWriter.WriteLine("\t{");

				for (int i = 0; i < enumLineList.Count; i++)
				{
					EnumLine enumLine = enumLineList[i];

					string name = enumLine.enumName;
					string assetAddress = enumLine.assetAddress;
					bool isNextLine = enumLine.isNextLine;

					if (isNextLine) streamWriter.WriteLine("");

					if (!string.IsNullOrEmpty(assetAddress)) streamWriter.WriteLine("\t\t" + assetAddress);
					streamWriter.WriteLine("\t\t" + name);
				}

				streamWriter.WriteLine("\t}");
				streamWriter.WriteLine("}");
			}
			AssetDatabase.Refresh();
		}
	}
}