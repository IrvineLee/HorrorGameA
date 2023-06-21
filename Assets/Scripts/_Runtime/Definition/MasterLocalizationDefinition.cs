using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.Localization;
using Personal.GameState;
using Personal.Setting.Game;
using Helper;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "MasterLocalizationDefinition", menuName = "ScriptableObjects/MasterLocalizationDefinition", order = 0)]
	[Serializable]
	public class MasterLocalizationDefinition : ScriptableObject
	{
		/// <summary>
		/// Fill up this class when there are more data localizations to be inserted.
		/// </summary>
		[Serializable]
		public class LocalizedGameData
		{
			[SerializeField] SupportedLanguageType language = SupportedLanguageType.English;
			[SerializeField] LocalizationGenericItem item = null;

			public SupportedLanguageType Language { get => language; }
			public LocalizationGenericItem Item { get => item; }
		}

		[SerializeField] List<LocalizedGameData> gameDataList = new();

		public IReadOnlyDictionary<SupportedLanguageType, LocalizedGameData> GameDataDictionary { get => dictionary; }

		Dictionary<SupportedLanguageType, LocalizedGameData> dictionary = new();
		GameData gameData;

		/// <summary>
		/// Initialize the definition.
		/// </summary>
		public void Initialize()
		{
			dictionary.Clear();
			foreach (var gameData in gameDataList)
			{
				dictionary.Add(gameData.Language, gameData);
			}

			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;
		}

		/// <summary>
		/// Get the current selected language data.
		/// </summary>
		/// <returns></returns>
		public LocalizedGameData LocalizedData()
		{
			var result = dictionary.GetOrDefault(gameData.SelectedLanguage);
			return result;
		}
	}
}