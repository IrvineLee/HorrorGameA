using System;
using UnityEngine;

using Personal.UI;
using Personal.InputProcessing;
using Personal.Localization;

namespace Personal.Setting.Game
{
	[Serializable]
	public class GameData
	{
		[SerializeField] float brightness = 0;
		[SerializeField] float cameraSensitivity = 1;
		[SerializeField] bool isInverLookHorizontal = false;
		[SerializeField] bool isInvertLookVertical = false;
		[SerializeField] bool isUSInteractButton = true;                                // x/o for confirm button
		[SerializeField] IconDisplayType iconDisplayType = IconDisplayType.Auto;
		[SerializeField] FontSizeType fontSizeType = FontSizeType.Normal;
		[SerializeField] SupportedLanguageType selectedLanguage = SupportedLanguageType.English;

		public float Brightness { get => brightness; set => brightness = value; }
		public float CameraSensitivity { get => cameraSensitivity; set => cameraSensitivity = value; }
		public bool IsInvertLookHorizontal { get => isInverLookHorizontal; set => isInverLookHorizontal = value; }
		public bool IsInvertLookVertical { get => isInvertLookVertical; set => isInvertLookVertical = value; }
		public bool IsUSInteractButton { get => isUSInteractButton; set => isUSInteractButton = value; }
		public IconDisplayType IconDisplayType { get => iconDisplayType; set => iconDisplayType = value; }
		public FontSizeType FontSizeType { get => fontSizeType; set => fontSizeType = value; }
		public SupportedLanguageType SelectedLanguage { get => selectedLanguage; set => selectedLanguage = value; }
	}
}