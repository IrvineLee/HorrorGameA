using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.Localization;
using Helper;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterLocalization : ScriptableObject, ISerializationCallbackReceiver
{
	public enum TableNameType
	{
		NameText = 0,
		DescriptionText,
	}

	[Serializable]
	public class Localization
	{
		public string NameText { get; private set; }
		public string DescriptionText { get; private set; }

		public Localization(string nameText, string descriptionText)
		{
			NameText = nameText;
			DescriptionText = descriptionText;
		}
	}

	public List<LocalizationTextEntity> NameTextEntities;
	public List<LocalizationTextEntity> DescriptionTextEntities;

	protected Dictionary<int, LocalizationTextEntity> nameTextDictionary = new();
	protected Dictionary<int, LocalizationTextEntity> descriptionTextDictionary = new();

	static Dictionary<SupportedLanguageType, Dictionary<int, Localization>> localizationDictionary = new();
	static Dictionary<int, Localization> activeLocalizedDictionary;

	List<int> idList = new();

	public void OnBeforeSerialize() { }

	public void OnAfterDeserialize()
	{
		foreach (var entity in NameTextEntities)
		{
			nameTextDictionary.Add(entity.id, entity);
		}

		foreach (var entity in DescriptionTextEntities)
		{
			descriptionTextDictionary.Add(entity.id, entity);
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

			foreach (int id in idList)
			{
				LocalizationTextEntity nameTextEntity = GetTableEntity(TableNameType.NameText, id);
				LocalizationTextEntity descriptionTextEntity = GetTableEntity(TableNameType.DescriptionText, id);

				idLocalizationDictionary.Add(id, new Localization(GetValue(language, nameTextEntity), GetValue(language, descriptionTextEntity)));
			}

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
	/// <param name="language"></param>
	/// <returns></returns>
	public static string Get(TableNameType tableNameType, int id)
	{
		if (!activeLocalizedDictionary.TryGetValue(id, out Localization localization)) return string.Empty;

		switch (tableNameType)
		{
			case TableNameType.NameText: return localization.NameText;
			case TableNameType.DescriptionText: return localization.DescriptionText;
		}

		return string.Empty;
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
			case TableNameType.NameText:
				return nameTextDictionary.GetOrDefault(id);
			case TableNameType.DescriptionText:
				return descriptionTextDictionary.GetOrDefault(id);
		}
		return null;
	}

	/// <summary>
	/// Get text value based on language selected.
	/// </summary>
	/// <param name="language"></param>
	/// <param name="entity"></param>
	/// <returns></returns>
	string GetValue(SupportedLanguageType language, LocalizationTextEntity entity)
	{
		switch (language)
		{
			case SupportedLanguageType.English: return entity.en;
			case SupportedLanguageType.Japanese: return entity.jp;
		}
		return "";
	}

	/// <summary>
	/// Get all unique id list.
	/// </summary>
	void GetAllIDList()
	{
		idList.Clear();
		foreach (var entity in NameTextEntities)
		{
			AddToIDList(entity.id);
		}

		foreach (var entity in DescriptionTextEntities)
		{
			AddToIDList(entity.id);
		}
	}

	/// <summary>
	/// Add unique id.
	/// </summary>
	/// <param name="id"></param>
	void AddToIDList(int id)
	{
		if (idList.Contains(id)) return;
		idList.Add(id);
	}
}
