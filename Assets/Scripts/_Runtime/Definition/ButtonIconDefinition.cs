using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;
using Helper;
using System.Text;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "ButtonIconDefinition", menuName = "ScriptableObjects/Input/ButtonIconDefinition", order = 0)]
	[Serializable]
	// Most probably you wanna add more string for textDisplay depending on the situation.
	// As of now it only handles the option's UI display.
	public class ButtonIconDefinition : ScriptableObject
	{
		[Serializable]
		public class ButtonIconInfo
		{
			[SerializeField] GenericButtonIconType genericButtonIconType = GenericButtonIconType.Dpad;
			[SerializeField] string textDisplay = "";

			public GenericButtonIconType GenericButtonIconType { get => genericButtonIconType; }
			public string TextDisplay { get => textDisplay; }
		}

		[SerializeField] List<ButtonIconInfo> ui_ButtonIconInfoList = new();

		public List<ButtonIconInfo> Ui_ButtonIconInfoList { get => ui_ButtonIconInfoList; }

		StringBuilder sb = new StringBuilder();
		List<string> textList = new List<string>();

		public List<string> GetAllText(List<ButtonIconInfo> buttonInfoList)
		{
			textList.Clear();

			foreach (var buttonInfo in buttonInfoList)
			{
				sb.Clear();
				string s = sb.Append("<sprite name=").
							Append(InputManager.Instance.IconInitials).
							Append(buttonInfo.GenericButtonIconType.GetStringValue()).Append("> ").
							Append(buttonInfo.TextDisplay).ToString();

				textList.Add(s);
			}

			return textList;
		}
	}
}

