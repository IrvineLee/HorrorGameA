using UnityEngine;
using TMPro;

namespace Lean.Localization
{
	/// <summary>This component will update a TMPro.TextMeshProUGUI component with localized text, or use a fallback if none is found.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(TextMeshProUGUI))]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Localized TextMeshProUGUI")]
	public class LeanLocalizedTextMeshProUGUI : LeanLocalizedBehaviour
	{
		[Tooltip("If PhraseName couldn't be found, this text will be used")]
		public string FallbackText;

		// This gets called every time the translation needs updating
		public override void UpdateTranslation(LeanTranslation translation)
		{
			if (this == null) return;

			// Get the TextMeshProUGUI component attached to this GameObject
			var text = GetComponent<TextMeshProUGUI>();

			// Use translation?
			if (translation != null && translation.Data is string)
			{
				text.text = LeanTranslation.FormatText((string)translation.Data, text.text, this, gameObject);
			}
			// Use fallback?
			else
			{
				text.text = LeanTranslation.FormatText(FallbackText, text.text, this, gameObject);
			}

			if (!string.IsNullOrEmpty(LeanLocalization.CurrentLanguageStr) &&
				LeanLocalization.CurrentLanguages.TryGetValue(LeanLocalization.CurrentLanguageStr, out var language))
			{
				if (language.FontAsset == null) return;
				text.font = language.FontAsset;
			}
		}

		protected virtual void Awake()
		{
			// Should we set FallbackText?
			if (string.IsNullOrEmpty(FallbackText) == true)
			{
				// Get the TextMeshProUGUI component attached to this GameObject
				var text = GetComponent<TextMeshProUGUI>();

				// Copy current text to fallback
				FallbackText = text.text;
			}
		}
	}
}