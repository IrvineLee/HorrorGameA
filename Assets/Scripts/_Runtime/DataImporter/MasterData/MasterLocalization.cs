using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.Localization;
using Helper;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterLocalization : ScriptableObject, ISerializationCallbackReceiver
{
	public enum TableNameTypes
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

	public IReadOnlyDictionary<string, Localization> activeLocalizedDictionary { get; private set; }

	protected Dictionary<string, LocalizationTextEntity> nameTextDictionary = new();
	protected Dictionary<string, LocalizationTextEntity> descriptionTextDictionary = new();

	protected Dictionary<SupportedLanguageType, Dictionary<string, Localization>> localizationDictionary = new();

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
	public void Initialize()
	{
		GetAllKeyList();
		localizationDictionary.Clear();

		Dictionary<string, Localization> keyLocalizationDictionary = new();

		foreach (SupportedLanguageType language in Enum.GetValues(typeof(SupportedLanguageType)))
		{
			foreach (var key in keyStr)
			{
				LocalizationTextEntity nameTextEntity = GetTableEntity(TableNameTypes.NameText, key);
				LocalizationTextEntity descriptionTextEntity = GetTableEntity(TableNameTypes.DescriptionText, key);

				keyLocalizationDictionary.Add(key, new Localization(GetValue(language, nameTextEntity), GetValue(language, descriptionTextEntity)));
			}

			localizationDictionary.Add(language, keyLocalizationDictionary);
			keyLocalizationDictionary = new();
		}
	}

	/// <summary>
	/// Update the current localization based on selected language.
	/// </summary>
	/// <param name="language"></param>
	public void UpdateActiveLanguage(SupportedLanguageType language)
	{
		activeLocalizedDictionary = localizationDictionary.GetOrDefault(language);
	}

	/// <summary>
	/// Get the localized data.
	/// </summary>
	/// <param name="language"></param>
	/// <returns></returns>
	public Localization Get(string key)
	{
		activeLocalizedDictionary.TryGetValue(key, out Localization localization);
		var result = activeLocalizedDictionary.GetOrDefault(key);

		return result;
	}

	/// <summary>
	/// Get table data from MasterLocalization.
	/// </summary>
	/// <param name="tableNameTypes"></param>
	/// <param name="key"></param>
	/// <returns></returns>
	LocalizationTextEntity GetTableEntity(TableNameTypes tableNameTypes, string key)
	{
		switch (tableNameTypes)
		{
			case TableNameTypes.NameText:
				return nameTextDictionary.GetOrDefault(key);
			case TableNameTypes.DescriptionText:
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
