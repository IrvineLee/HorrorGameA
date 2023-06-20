using System;
using UnityEngine;

using System.Collections.Generic;
using Personal.Localization;

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
		public class GameData
		{
			[SerializeField] SupportedLanguageType language = SupportedLanguageType.English;
			[SerializeField] LocalizationGenericItem item = null;

			public SupportedLanguageType Language { get => language; }
			public LocalizationGenericItem Item { get => item; }
		}

		[SerializeField] List<GameData> gameDataList = new();

		public IReadOnlyDictionary<SupportedLanguageType, GameData> AudioDictionary { get => dictionary; }

		Dictionary<SupportedLanguageType, GameData> dictionary = new();

		public void Initialize()
		{
			dictionary.Clear();
			foreach (var gameData in gameDataList)
			{
				dictionary.Add(gameData.Language, gameData);
			}
		}
	}
}