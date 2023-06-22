using Personal.GameState;
using Lean.Localization;
using TMPro;

namespace Personal.UI
{
	public class SetFontAsset : GameInitialize
	{
		TextMeshProUGUI tmp;

		protected override void PreInitialize()
		{
			tmp = GetComponent<TextMeshProUGUI>();
		}

		public void HandleChange()
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
