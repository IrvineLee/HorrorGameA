using System;
using System.Collections.Generic;
using UnityEngine;

using Helper;

namespace Personal.Localization
{
	[ExcelAsset(AssetPath = "Data/MasterData/Data")]
	public class MasterReadFile : ScriptableObject, ISerializationCallbackReceiver
	{
		public List<ReadFileEntity> En;
		public List<ReadFileEntity> Jp;

		protected Dictionary<int, ReadFileEntity> enDictionary = new();
		protected Dictionary<int, ReadFileEntity> jpDictionary = new();

		static Dictionary<SupportedLanguageType, Dictionary<int, ReadFileEntity>> localizationDictionary = new();
		static Dictionary<int, ReadFileEntity> activeLocalizedDictionary;

		public void OnBeforeSerialize() { }

		public void OnAfterDeserialize()
		{
			localizationDictionary.Clear();

			InitDictionary(SupportedLanguageType.English, En, enDictionary);
			InitDictionary(SupportedLanguageType.Japanese, Jp, jpDictionary);
		}

		/// <summary>
		/// Set the current localization based on selected language.
		/// </summary>
		/// <param name="language"></param>
		public static void SetActiveLanguage(SupportedLanguageType language)
		{
			activeLocalizedDictionary = localizationDictionary.GetOrDefault(language);
		}

		void InitDictionary(SupportedLanguageType language, List<ReadFileEntity> readFileList, Dictionary<int, ReadFileEntity> dictionary)
		{
			dictionary.Clear();
			foreach (var entity in readFileList)
			{
				dictionary.Add(entity.id, entity);
			}

			localizationDictionary.Add(language, dictionary);
		}

		/// <summary>
		/// Get the localized data.
		/// </summary>
		/// <param name="language"></param>
		/// <returns></returns>
		public static ReadFileEntity Get(int id)
		{
			if (!activeLocalizedDictionary.TryGetValue(id, out ReadFileEntity localization)) return null;
			return localization;
		}
	}
}