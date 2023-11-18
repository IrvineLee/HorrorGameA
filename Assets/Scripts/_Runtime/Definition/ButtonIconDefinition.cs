using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;
using Helper;
using System.Text;
using Personal.UI;
using Personal.UI.Option;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "ButtonIconDefinition", menuName = "ScriptableObjects/Input/ButtonIconDefinition", order = 0)]
	[Serializable]
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

		[SerializeField] List<ButtonIconInfo> uiOption_ButtonIconInfoList = new();
		[SerializeField] List<ButtonIconInfo> uiDialogue_ButtonIconInfoList = new();

		StringBuilder sb = new StringBuilder();
		List<string> textList = new List<string>();

		public List<string> GetCurrentInterfaceText(UIInterfaceType uiInterfaceType, bool isXInteract = true)
		{
			textList.Clear();

			var buttonIconInfoList = uiOption_ButtonIconInfoList;
			switch (uiInterfaceType)
			{
				case UIInterfaceType.Dialogue: buttonIconInfoList = uiDialogue_ButtonIconInfoList; break;
			}

			foreach (var buttonInfo in buttonIconInfoList)
			{
				string buttonStr = buttonInfo.GenericButtonIconType.GetStringValue();

				// Try to get the swap of interact button if exist.
				string confirmCancelStr = GetConfirmCancelStr(buttonInfo, isXInteract);
				if (!string.IsNullOrEmpty(confirmCancelStr)) buttonStr = confirmCancelStr;

				sb.Clear();
				string s = sb.Append("<sprite name=").
							Append(InputManager.Instance.IconInitials).
							Append(buttonStr).Append("> ").
							Append(buttonInfo.TextDisplay).ToString();

				textList.Add(s);
			}

			return textList;
		}

		/// <summary>
		/// Handle the swap of interact button.
		/// </summary>
		/// <param name="buttonInfo"></param>
		/// <returns></returns>
		string GetConfirmCancelStr(ButtonIconInfo buttonInfo, bool isXInteract)
		{
			if (isXInteract ||
				(buttonInfo.GenericButtonIconType != GenericButtonIconType.Button_East &&
				buttonInfo.GenericButtonIconType != GenericButtonIconType.Button_South))
			{
				return "";
			}

			if (buttonInfo.GenericButtonIconType == GenericButtonIconType.Button_East)
			{
				return GenericButtonIconType.Button_South.GetStringValue();
			}
			return GenericButtonIconType.Button_East.GetStringValue();
		}
	}
}

