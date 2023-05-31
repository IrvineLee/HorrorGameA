using System;
using UnityEngine;

using Personal.UI;

namespace Personal.Setting.Game
{
	[Serializable]
	public class GameData
	{
		[SerializeField] float brightness = 0;
		[SerializeField] float cameraSensitivity = 1;
		[SerializeField] bool isInverLookHorizontal = false;
		[SerializeField] bool isInvertLookVertical = false;
		[SerializeField] int gamepadIconIndex = 0;
		[SerializeField] FontSizeType fontSizeType = FontSizeType.Normal;
		[SerializeField] bool isSubtitle = true;

		public float Brightness { get => brightness; set => brightness = value; }
		public float CameraSensitivity { get => cameraSensitivity; set => cameraSensitivity = value; }
		public bool IsInvertLookHorizontal { get => isInverLookHorizontal; set => isInverLookHorizontal = value; }
		public bool IsInvertLookVertical { get => isInvertLookVertical; set => isInvertLookVertical = value; }
		public int GamepadIconIndex { get => gamepadIconIndex; set => gamepadIconIndex = value; }
		public FontSizeType FontSizeType { get => fontSizeType; set => fontSizeType = value; }
		public bool IsSubtitle { get => isSubtitle; set => isSubtitle = value; }
	}
}