using System.Collections.Generic;
using UnityEngine;

using Helper;
using Personal.Data;

namespace Personal.Localization
{
	[ExcelAsset(AssetPath = "Data/MasterData/Data")]
	public class MasterReadFile : MasterGeneric<ReadFileEntity, int>
	{
		public List<ReadFileEntity> Jp;

		protected Dictionary<int, ReadFileEntity> jpDictionary = new();

		static Dictionary<SupportedLanguageType, Dictionary<int, ReadFileEntity>> localizationDictionary = new();
		static Dictionary<int, ReadFileEntity> activeLocalizedDictionary;

		public override void OnBeforeSerialize() { }

		public override void OnAfterDeserialize()
		{
			localizationDictionary.Clear();

			InitDictionary(SupportedLanguageType.English, Entities, dictionary);
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
		public override ReadFileEntity Get(int id)
		{
			if (!activeLocalizedDictionary.TryGetValue(id, out ReadFileEntity localization)) return null;
			return localization;
		}
	}
}