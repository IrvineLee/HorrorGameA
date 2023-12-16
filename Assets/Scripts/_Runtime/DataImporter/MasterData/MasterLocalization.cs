using System;
using System.Collections.Generic;
using UnityEngine;

using Helper;
using Personal.Entity;

namespace Personal.Localization
{
	[ExcelAsset(AssetPath = "Data/MasterData/Data")]
	public class MasterLocalization : ScriptableObject, ISerializationCallbackReceiver
	{
		public enum TableNameType
		{
			NameText = 0,
			DescriptionText,
			QuestName,
			QuestDescriptionText,
		}

		public List<LocalizationNameTextEntity> NameEntities;
		public List<LocalizationNameTextEntity> DescriptionEntities;

		public List<LocalizationNameTextEntity> QuestNameEntities;
		public List<LocalizationQuestTextEntity> QuestDescriptionEntities;

		protected Dictionary<int, LocalizationNameTextEntity> nameTextDictionary = new();
		protected Dictionary<int, LocalizationNameTextEntity> descriptionTextDictionary = new();

		protected Dictionary<int, LocalizationNameTextEntity> questNameTextDictionary = new();
		protected Dictionary<int, LocalizationQuestTextEntity> questDescriptionTextDictionary = new();

		static Dictionary<SupportedLanguageType, Dictionary<int, Localization>> localizationDictionary = new();
		static Dictionary<int, Localization> activeLocalizedDictionary;

		List<int> idList = new();

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize()
		{
			nameTextDictionary.Clear();
			foreach (var entity in NameEntities)
			{
				nameTextDictionary.Add(entity.id, entity);
			}

			descriptionTextDictionary.Clear();
			foreach (var entity in DescriptionEntities)
			{
				descriptionTextDictionary.Add(entity.id, entity);
			}

			questNameTextDictionary.Clear();
			foreach (var entity in QuestNameEntities)
			{
				questNameTextDictionary.Add(entity.id, entity);
			}

			questDescriptionTextDictionary.Clear();
			foreach (var entity in QuestDescriptionEntities)
			{
				questDescriptionTextDictionary.Add(entity.id, entity);
			}
		}

		/// <summary>
		/// Initialize the value into dictionary.
		/// </summary>
		public void InitializeAllLanguages()
		{
			GetAllIDList();
			localizationDictionary.Clear();

			foreach (SupportedLanguageType language in Enum.GetValues(typeof(SupportedLanguageType)))
			{
				Dictionary<int, Localization> idLocalizationDictionary = new();

				InitializeID(language, idLocalizationDictionary);
				localizationDictionary.Add(language, idLocalizationDictionary);
			}
		}

		/// <summary>
		/// Set the current localization based on selected language.
		/// </summary>
		/// <param name="language"></param>
		public static void SetActiveLanguage(SupportedLanguageType language)
		{
			activeLocalizedDictionary = localizationDictionary.GetOrDefault(language);
		}

		/// <summary>
		/// Get the localized data.
		/// </summary>
		public static string Get(TableNameType tableNameType, int id)
		{
			if (!activeLocalizedDictionary.TryGetValue(id, out Localization localization)) return string.Empty;

			switch (tableNameType)
			{
				case TableNameType.NameText: return ((LocalizationSingle)localization).NameText;
				case TableNameType.DescriptionText: return ((LocalizationSingle)localization).DescriptionText;
				case TableNameType.QuestName: return ((LocalizationMultiple)localization).NameText;
			}

			return string.Empty;
		}

		/// <summary>
		/// Get the localized data.
		/// </summary>
		public static List<string> GetList(TableNameType tableNameType, int id)
		{
			if (!activeLocalizedDictionary.TryGetValue(id, out Localization localization)) return null;

			var localized = ((LocalizationMultiple)localization);
			switch (tableNameType)
			{
				case TableNameType.QuestDescriptionText: return localized.DescriptionTextList;
			}

			return null;
		}

		/// <summary>
		/// Initialize the IDs for specified language.
		/// </summary>
		void InitializeID(SupportedLanguageType language, Dictionary<int, Localization> idLocalizationDictionary)
		{
			var singleStrFunc = LocalizeSingleStr();

			foreach (int id in idList)
			{
				if (IsNameAndDescription(id, language, idLocalizationDictionary, singleStrFunc)) continue;
				else if (IsQuestAndDescription(id, language, idLocalizationDictionary, singleStrFunc)) continue;
			}
		}

		/// <summary>
		/// Handle name and description.
		/// </summary>
		bool IsNameAndDescription(int id, SupportedLanguageType language, Dictionary<int, Localization> dictionary, List<Func<LocalizationTextEntity, string>> singleStrFunc)
		{
			LocalizationTextEntity nameTextEntity = GetTableEntity(TableNameType.NameText, id);
			LocalizationTextEntity descriptionTextEntity = GetTableEntity(TableNameType.DescriptionText, id);

			string nameStr = nameTextEntity == null ? "" : GetValue(language, nameTextEntity, singleStrFunc);
			string descriptionStr = nameTextEntity == null ? "" : GetValue(language, descriptionTextEntity, singleStrFunc);

			if (!string.IsNullOrEmpty(nameStr) || !string.IsNullOrEmpty(descriptionStr))
			{
				Localization localization = new LocalizationSingle(nameStr, descriptionStr);
				dictionary.Add(id, localization);

				return true;
			}
			return false;
		}

		/// <summary>
		/// Handle quest and description.
		/// </summary>
		bool IsQuestAndDescription(int id, SupportedLanguageType language, Dictionary<int, Localization> dictionary, List<Func<LocalizationTextEntity, string>> singleStrFunc)
		{
			LocalizationTextEntity questNameTextEntity = GetTableEntity(TableNameType.QuestName, id);
			LocalizationTextEntity questDescriptionTextEntity = GetTableEntity(TableNameType.QuestDescriptionText, id);

			var questDescriptionFunc = LocalizeMultipleStr(TableNameType.QuestDescriptionText);

			string questNameStr = questNameTextEntity == null ? "" : GetValue(language, questNameTextEntity, singleStrFunc);
			List<string> questDescriptionStrList = questDescriptionTextEntity == null ? new() : GetValue(language, questDescriptionTextEntity, questDescriptionFunc);

			if (!string.IsNullOrEmpty(questNameStr))
			{
				Localization localization = new LocalizationMultiple(questNameStr, questDescriptionStrList);
				dictionary.Add(id, localization);

				return true;
			}
			return false;
		}

		/// <summary>
		/// Get table data from MasterLocalization.
		/// </summary>
		/// <param name="tableNameTypes"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		LocalizationTextEntity GetTableEntity(TableNameType tableNameTypes, int id)
		{
			switch (tableNameTypes)
			{
				case TableNameType.NameText: return nameTextDictionary.GetOrDefault(id);
				case TableNameType.DescriptionText: return descriptionTextDictionary.GetOrDefault(id);
				case TableNameType.QuestName: return questNameTextDictionary.GetOrDefault(id);
				case TableNameType.QuestDescriptionText: return questDescriptionTextDictionary.GetOrDefault(id);
			}
			return null;
		}

		/// <summary>
		/// Get text value based on language selected.
		/// </summary>
		/// <param name="language"></param>
		/// <param name="entity"></param>
		/// <param name="funcList">Some entity might use different parameter's name, so create the initialization somewhere else.</param>
		/// <returns></returns>
		string GetValue(SupportedLanguageType language, LocalizationTextEntity entity, List<Func<LocalizationTextEntity, string>> funcList)
		{
			switch (language)
			{
				case SupportedLanguageType.English: return funcList[0].Invoke(entity);
				case SupportedLanguageType.Japanese: return funcList[1].Invoke(entity);
			}
			return "";
		}

		/// <summary>
		/// Get text value based on language selected.
		/// </summary>
		/// <param name="language"></param>
		/// <param name="entity"></param>
		/// <param name="funcList">Some entity might use different parameter's name, so create the initialization somewhere else.</param>
		/// <returns></returns>
		List<string> GetValue(SupportedLanguageType language, LocalizationTextEntity entity, List<Func<LocalizationTextEntity, List<string>>> funcList)
		{
			switch (language)
			{
				case SupportedLanguageType.English: return funcList[0].Invoke(entity);
				case SupportedLanguageType.Japanese: return funcList[1].Invoke(entity);
			}
			return new();
		}

		/// <summary>
		/// Localize single string that uses the language initials as the name.
		/// </summary>
		/// <returns></returns>
		List<Func<LocalizationTextEntity, string>> LocalizeSingleStr()
		{
			List<Func<LocalizationTextEntity, string>> funcList = new();

			funcList.Add((entity) => { return ((LocalizationNameTextEntity)entity).en; });
			funcList.Add((entity) => { return ((LocalizationNameTextEntity)entity).jp; });

			return funcList;
		}

		/// <summary>
		/// Localize multiple strings.
		/// </summary>
		/// <returns></returns>
		List<Func<LocalizationTextEntity, List<string>>> LocalizeMultipleStr(TableNameType tableNameType)
		{
			List<string> strList = new();
			List<Func<LocalizationTextEntity, List<string>>> funcList = new();

			switch (tableNameType)
			{
				case TableNameType.QuestDescriptionText: return LocalizeQuest(strList, funcList);
			}

			return funcList;
		}

		/// <summary>
		/// Localize quest description.
		/// </summary>
		/// <param name="strList"></param>
		/// <param name="funcList"></param>
		/// <returns></returns>
		List<Func<LocalizationTextEntity, List<string>>> LocalizeQuest(List<string> strList, List<Func<LocalizationTextEntity, List<string>>> funcList)
		{
			funcList.Add((entity) => { return strList = new() { ((LocalizationQuestTextEntity)entity).en01, ((LocalizationQuestTextEntity)entity).en02, ((LocalizationQuestTextEntity)entity).en03 }; });
			funcList.Add((entity) => { return strList = new() { ((LocalizationQuestTextEntity)entity).jp01, ((LocalizationQuestTextEntity)entity).jp02, ((LocalizationQuestTextEntity)entity).jp03 }; });

			return funcList;
		}

		/// <summary>
		/// Get all unique id list.
		/// </summary>
		void GetAllIDList()
		{
			idList.Clear();

			AddToIDList(NameEntities.ConvertAll(x => (GenericEntity)x));
			AddToIDList(DescriptionEntities.ConvertAll(x => (GenericEntity)x));
			AddToIDList(QuestDescriptionEntities.ConvertAll(x => (GenericEntity)x));
		}

		/// <summary>
		/// Add unique id.
		/// </summary>
		/// <param name="id"></param>
		void AddToIDList(List<GenericEntity> localizationTextEntity)
		{
			foreach (var entity in localizationTextEntity)
			{
				if (idList.Contains(entity.id)) return;
				idList.Add(entity.id);
			}
		}
	}
}