using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Personal.Entity;

namespace Personal.Data
{
	/// <summary>
	/// 
	/// </summary>
	[CreateAssetMenu(fileName = "ExcelToEnum", menuName = "ScriptableObjects/ExcelToEnum", order = 0)]
	public class ExcelToEnum : ScriptableObject
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
		public class ExcelEnumData
		{
			[SerializeField] UnityEngine.Object definition;
			[SerializeField] DataEnumType dataEnumType;

			public UnityEngine.Object Definition { get => definition; }
			public DataEnumType DataEnumType { get => dataEnumType; }
		}

		[Serializable]
		public class EnumLine
		{
			[SerializeField] string enumName;
			[SerializeField] string assetAddress;
			[SerializeField] bool isNextLine;

			public string EnumName { get => enumName; }
			public string AssetAddress { get => assetAddress; }
			public bool IsNextLine { get => isNextLine; }


			public EnumLine(string enumName, string assetAddress, bool isNextLine)
			{
				this.enumName = enumName;
				this.assetAddress = assetAddress;
				this.isNextLine = isNextLine;
			}
		}

		[SerializeField] List<ExcelEnumData> excelToEnumList = new();

		[ContextMenu("Refresh Enum")]
		void UpdateEnum()
		{
			ConvertToEnum();
		}

		void ConvertToEnum()
		{
			foreach (var excelToEnum in excelToEnumList)
			{
				var definition = excelToEnum.Definition;

				switch (excelToEnum.DataEnumType)
				{
					case DataEnumType.Window: HandleEnumGeneration<MasterWindowUI, WindowUIEntity>(definition, "Personal.UI.Window", "WindowUIType", "The reason a window appears."); break;
					case DataEnumType.BGM: HandleEnumGeneration<MasterBGM, BGMEntity>(definition, "Personal.Setting.Audio", "AudioBGMType"); break;
					case DataEnumType.SFX: HandleEnumGeneration<MasterSFX, SFXEntity>(definition, "Personal.Setting.Audio", "AudioSFXType"); break;
					case DataEnumType.Item: HandleEnumGeneration<MasterItem, ItemEntity>(definition, "Personal.Item", "ItemType", "", true); break;
					case DataEnumType.Quest: HandleEnumGeneration<MasterQuest, QuestEntity>(definition, "Personal.Quest", "QuestType"); break;
					case DataEnumType.Achievement: HandleEnumGeneration<MasterAchievement, AchievementEntity>(definition, "Personal.Achievement", "AchievementType"); break;
					case DataEnumType.ReadFile: HandleEnumGeneration<MasterReadFile, ReadFileEntity>(definition, "Personal.UI", "ReadFileType"); break;
					case DataEnumType.KeyEvent: HandleEnumGeneration<MasterKeyEvent, KeyEventEntity>(definition, "Personal.KeyEvent", "KeyEventType"); break;
				}
			}

			Debug.Log("Completed!");
		}

		void HandleEnumGeneration<T1, T2>(UnityEngine.Object definition, string namespaceName, string enumTypeStr, string comments = "", bool isAddAssetAdress = false)
			where T1 : MasterGeneric<T2, int> where T2 : GenericNameEntity
		{
			T1 currentMasterData = (T1)definition;

			var scriptArray = AssetDatabase.FindAssets(enumTypeStr);
			var path = "";

			foreach (var script in scriptArray)
			{
				var currentPath = AssetDatabase.GUIDToAssetPath(script);
				var currentObject = AssetDatabase.LoadAssetAtPath(currentPath, typeof(UnityEngine.Object));

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
				if (enumLineList.Where((x => !string.IsNullOrEmpty(x.AssetAddress))).Any())
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

					string name = enumLine.EnumName;
					string assetAddress = enumLine.AssetAddress;
					bool isNextLine = enumLine.IsNextLine;

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