using System;
using UnityEngine;

namespace Personal.Localization
{
	[Serializable]
	public class LocalizationSingle : Localization
	{
		public string NameText { get; private set; }
		public string DescriptionText { get; private set; }

		public LocalizationSingle(string nameText, string descriptionText)
		{
			NameText = nameText;
			DescriptionText = descriptionText;
		}
	}
}