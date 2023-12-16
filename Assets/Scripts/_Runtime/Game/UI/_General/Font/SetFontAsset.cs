using UnityEngine;

using Lean.Localization;
using TMPro;
using Personal.Localization;
using Personal.UI.Option;

namespace Personal.UI
{
	public class SetFontAsset : MonoBehaviour
	{
		TextMeshProUGUI tmp;

		void Awake()
		{
			tmp = GetComponent<TextMeshProUGUI>();
			OptionGameUI.OnLanguageChangedEvent += OnLanguageChanged;
		}

		void OnEnable()
		{
			HandleChange();
		}

		void OnLanguageChanged(SupportedLanguageType supportedLanguageType)
		{
			HandleChange();
		}

		void HandleChange()
		{
			if (!string.IsNullOrEmpty(LeanLocalization.CurrentLanguageStr) &&
				LeanLocalization.CurrentLanguages.TryGetValue(LeanLocalization.CurrentLanguageStr, out var language))
			{
				if (language.FontAsset == null) return;
				tmp.font = language.FontAsset;
			}
		}

		void OnDestroy()
		{
			OptionGameUI.OnLanguageChangedEvent -= OnLanguageChanged;
		}
	}
}
