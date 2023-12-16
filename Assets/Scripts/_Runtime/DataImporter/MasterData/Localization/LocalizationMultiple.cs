using System.Collections.Generic;
using System;
using UnityEngine;

namespace Personal.Localization
{
	[Serializable]
	public class LocalizationMultiple : Localization
	{
		public string NameText { get; private set; }
		public List<string> DescriptionTextList { get; private set; } = new();

		public LocalizationMultiple(string nameText, List<string> descriptionTextList)
		{
			NameText = nameText;
			DescriptionTextList = descriptionTextList;
		}
	}
}