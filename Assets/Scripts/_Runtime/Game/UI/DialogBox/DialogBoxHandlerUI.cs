using System.Collections.Generic;
using UnityEngine;
using System;

namespace Personal.UI.Dialog
{
	public class DialogBoxHandlerUI : MonoBehaviour
	{
		public enum DialogDisplayType
		{
			DialogButtonConfirmationBox = 0,
		}

		public enum ButtonDisplayType
		{
			One_Ok = 0,
			Two_YesNo,
			Three,
		}

		[Serializable]
		public class ButtonInfo
		{
			[SerializeField] ButtonDisplayType buttonType = ButtonDisplayType.One_Ok;
			[SerializeField] DialogBoxButtonPress buttonPress = null;

			public ButtonDisplayType ButtonType { get => buttonType; }
			public DialogBoxButtonPress ButtonPress { get => buttonPress; }
		}

		[SerializeField] List<DialogBoxMenuUI> dialogBoxMenuUIList = new List<DialogBoxMenuUI>();
		[SerializeField] List<ButtonInfo> buttonInfoList = new List<ButtonInfo>();

		public Stack<DialogBoxMenuUI> DialogBoxStack { get; } = new();

		Dictionary<DialogDisplayType, DialogBoxMenuUI> dialogBoxUIDictionary = new();
		Dictionary<ButtonDisplayType, DialogBoxButtonPress> buttonInfoDictionary = new();

		public void Initialize()
		{
			foreach (var dialogBox in dialogBoxMenuUIList)
			{
				_ = dialogBox.Initialize();
				dialogBoxUIDictionary.Add(dialogBox.DialogType, dialogBox);
			}

			foreach (var buttonInfo in buttonInfoList)
			{
				_ = buttonInfo.ButtonPress.Initialize();
				buttonInfoDictionary.Add(buttonInfo.ButtonType, buttonInfo.ButtonPress);
			}
		}

		/// <summary>
		/// Get the type of dialog display.
		/// </summary>
		/// <param name="buttonDisplayType"></param>
		/// <returns></returns>
		public DialogBoxButtonPress GetDialogButtonPress(ButtonDisplayType buttonDisplayType)
		{
			buttonInfoDictionary.TryGetValue(buttonDisplayType, out DialogBoxButtonPress buttonPress);
			return buttonPress;
		}

		/// <summary>
		/// Disable all dialog displays.
		/// </summary>
		public void DisableAllDialogDisplays()
		{
			foreach (var dialogBox in dialogBoxMenuUIList)
			{
				dialogBox.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Disable all dialog buttons.
		/// </summary>
		public void DisableAllDialogButtons()
		{
			foreach (var buttonInfo in buttonInfoList)
			{
				buttonInfo.ButtonPress.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Open dialog box with certain parameters.
		/// </summary>
		public void OpenDialogBox(DialogDisplayType dialogType, ButtonDisplayType buttonType, string title, string message,
								  Action action01 = default, Action action02 = default, Action action03 = default)
		{
			if (!dialogBoxUIDictionary.TryGetValue(dialogType, out DialogBoxMenuUI dialogBoxMenuUI)) return;

			DialogBoxStack.Push(dialogBoxMenuUI);

			dialogBoxMenuUI.gameObject.SetActive(true);
			if (dialogType == DialogDisplayType.DialogButtonConfirmationBox)
			{
				SetDialogButton(dialogBoxMenuUI, buttonType, title, message, action01, action02, action03);
			}
		}

		void SetDialogButton(DialogBoxMenuUI dialogBoxMenuUI, ButtonDisplayType buttonType, string title, string message,
							Action action01, Action action02, Action action03)
		{
			if (buttonType == ButtonDisplayType.Two_YesNo)
			{
				dialogBoxMenuUI.SetTwoButtonYesNo(dialogBoxMenuUI, title, message, action01, action02);
				return;
			}
			else if (buttonType == ButtonDisplayType.Three)
			{
				dialogBoxMenuUI.SetThreeButton(dialogBoxMenuUI, title, message, action01, action02, action03);
				return;
			}

			dialogBoxMenuUI.SetOneButtonOk(dialogBoxMenuUI, title, message, action01);
		}
	}
}
