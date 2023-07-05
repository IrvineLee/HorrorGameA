using UnityEngine;

using Lean.Localization;
using TMPro;

namespace Personal.UI
{
	public class SetFontAsset : MonoBehaviour
	{
		TextMeshProUGUI tmp;

		void Awake()
		{
			tmp = GetComponent<TextMeshProUGUI>();
		}

		void OnEnable()
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
	}
}
