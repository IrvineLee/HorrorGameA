using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Lean.Localization;

namespace Personal.UI
{
	public class UISelectionDropdownExtension : UISelectionDropdown
	{
		[Tooltip("This updates the value of the text from strList. " +
			"Useful when needing to show user different value when using strList for different purposes.")]
		[SerializeField] List<string> displayStrList = new();

		public List<string> DisplayStrList { get => displayStrList; }

		protected override void Initialize()
		{
			base.Initialize();

			for (int i = 0; i < stringTMPList.Count; i++)
			{
				TextMeshProUGUI tmp = stringTMPList[i];
				tmp.text = displayStrList[i];
				ExtraSetup(StringList[i], tmp);
			}
		}

		/// <summary>
		/// Do extra setup if needed.
		/// </summary>
		/// <param name="str">The real value. Typically is the value from strList.</param>
		/// <param name="tmp">The instantiated tmp.</param>
		protected virtual void ExtraSetup(string str, TextMeshProUGUI tmp)
		{
			LeanLocalization.CurrentLanguages.TryGetValue(str, out var language);
			tmp.font = language.FontAsset;
		}
	}
}
