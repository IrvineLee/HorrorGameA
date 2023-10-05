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

	protected Dictionary<string, LocalizationTextEntity> nameTextDictionary = new();
	protected Dictionary<string, LocalizationTextEntity> descriptionTextDictionary = new();

	static Dictionary<SupportedLanguageType, Dictionary<string, Localization>> localizationDictionary = new();
	static Dictionary<string, Localization> activeLocalizedDictionary;

	HashSet<string> keyStr = new();

	public void OnBeforeSerialize() { }

	public void OnAfterDeserialize()
	{
		foreach (var entity in NameTextEntities)
		{
			nameTextDictionary.Add(entity.key, entity);
		}

		foreach (var entity in DescriptionTextEntities)
		{
			descriptionTextDictionary.Add(entity.key, entity);
		}
	}

	/// <summary>
	/// Initialize the value into dictionary.
	/// </summary>
	public void InitializeAllLanguages()
	{
		GetAllKeyList();
		localizationDictionary.Clear();

		foreach (SupportedLanguageType language in Enum.GetValues(typeof(SupportedLanguageType)))
		{
			Dictionary<string, Localization> keyLocalizationDictionary = new();

			foreach (var key in keyStr)
			{
				LocalizationTextEntity nameTextEntity = GetTableEntity(TableNameType.NameText, key);
				LocalizationTextEntity descriptionTextEntity = GetTableEntity(TableNameType.DescriptionText, key);

				keyLocalizationDictionary.Add(key, new Localization(GetValue(language, nameTextEntity), GetValue(language, descriptionTextEntity)));
			}

			localizationDictionary.Add(language, keyLocalizationDictionary);
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
	public static string Get(TableNameType tableNameType, string key)
	{
		if (!activeLocalizedDictionary.TryGetValue(key, out Localization localization)) return string.Empty;

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
	/// <param name="key"></param>
	/// <returns></returns>
	LocalizationTextEntity GetTableEntity(TableNameType tableNameTypes, string key)
	{
		switch (tableNameTypes)
		{
			case TableNameType.NameText:
				return nameTextDictionary.GetOrDefault(key);
			case TableNameType.DescriptionText:
				return descriptionTextDictionary.GetOrDefault(key);
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
	/// Get all unique key list.
	/// </summary>
	void GetAllKeyList()
	{
		keyStr.Clear();
		foreach (var entity in NameTextEntities)
		{
			AddToKeyList(entity.key, keyStr);
		}

		foreach (var entity in DescriptionTextEntities)
		{
			AddToKeyList(entity.key, keyStr);
		}
	}

	/// <summary>
	/// Add unique key.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="keyStr"></param>
	void AddToKeyList(string key, HashSet<string> keyStr)
	{
		if (keyStr.Contains(key)) return;
		keyStr.Add(key);
	}
}
