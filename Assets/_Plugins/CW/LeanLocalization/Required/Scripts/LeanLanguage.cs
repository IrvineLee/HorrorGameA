using UnityEngine;
using System.Collections.Generic;
using Lean.Common;
using CW.Common;
using TMPro;

namespace Lean.Localization
{
	/// <summary>This contains information about a language, and any of its optional cultures.</summary>
	[System.Serializable]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[HelpURL(LeanLocalization.HelpUrlPrefix + "LeanLanguage")]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Language")]
	public class LeanLanguage : LeanSource
	{
		[SerializeField] private TMP_FontAsset fontAsset;
		[SerializeField] private string translationCode;
		[SerializeField] private List<string> cultures;

		public TMP_FontAsset FontAsset { get { return fontAsset; } set { fontAsset = value; } }

		/// <summary>The language code used for auto translation.</summary>
		public string TranslationCode { get { return translationCode; } set { translationCode = value; } }

		/// <summary>This culture names for this language (e.g. en-GB, en-US).</summary>
		public List<string> Cultures { get { if (cultures == null) cultures = new List<string>(); return cultures; } }

		public override void Register()
		{
			if (LeanLocalization.CurrentLanguages.ContainsKey(name) == false)
			{
				LeanLocalization.CurrentLanguages.Add(name, this);
			}

			TryAddAlias(name, name);

			if (cultures != null)
			{
				foreach (var culture in cultures)
				{
					TryAddAlias(culture, name);
				}

			}
		}

		private void TryAddAlias(string key, string value)
		{
			if (LeanLocalization.CurrentAliases.ContainsKey(key) == false)
			{
				LeanLocalization.CurrentAliases.Add(key, value);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Localization.Editor
{
	using UnityEditor;
	using TARGET = LeanLanguage;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanLanguage_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("fontAsset", "Font asset for this language.");
			Draw("translationCode", "The language code used for auto translation.");

			Separator();

			Draw("cultures", "This culture names for this language (e.g. en-GB, en-US).");
		}

		[MenuItem("Assets/Create/Lean/Localization/Lean Language")]
		private static void CreateLanguage()
		{
			CwHelper.CreatePrefabAsset("New Language").AddComponent<LeanLanguage>();
		}
	}
}
#endif