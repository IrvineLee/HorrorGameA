using System;
using UnityEngine;

using TMPro;
using static Personal.UI.Dialog.DialogBoxHandlerUI;

namespace Personal.UI.Dialog
{
	public class DialogBoxMenuUI : DialogBoxMenuUIBase
	{
		[SerializeField] DialogDisplayType dialogType = DialogDisplayType.DialogButtonConfirmationBox;

		public DialogDisplayType DialogType { get => dialogType; }
		public Action CancelAction { get; private set; }

		[SerializeField] TextMeshProUGUI titleTMP = null;
		[SerializeField] TextMeshProUGUI descriptionTMP = null;

		public virtual void SetOneButtonOk(DialogBoxMenuUI dialogBoxMenuUI, string title, string message, Action action, string buttonText = default)
		{
			DialogBoxButtonPress buttonPress = dialogBoxHandlerUI.GetDialogButtonPress(ButtonDisplayType.One_Ok);

			Action addListenerAction = () =>
			{
				Action onPressed = SetupOnButtonPressedAction(dialogBoxMenuUI, buttonPress, action);
				CancelAction = onPressed;

				// Enable the correct buttons.
				buttonPress.AddListenerToButtonOnce(onPressed, buttonText);
				buttonPress.gameObject.SetActive(true);
			};
			SetupButton(title, message, addListenerAction);
		}

		public virtual void SetTwoButtonYesNo(DialogBoxMenuUI dialogBoxMenuUI, string title, string message, Action action01, Action action02,
											   string buttonText01 = default, string buttonText02 = default)
		{
			DialogBoxButtonPress buttonPress = dialogBoxHandlerUI.GetDialogButtonPress(ButtonDisplayType.Two_YesNo);

			Action addListenerAction = () =>
			{
				Action onPressed01 = SetupOnButtonPressedAction(dialogBoxMenuUI, buttonPress, action01);
				Action onPressed02 = SetupOnButtonPressedAction(dialogBoxMenuUI, buttonPress, action02);
				CancelAction = onPressed02;

				// Enable the correct buttons.
				buttonPress.AddListenerToButtonOnce(onPressed01, onPressed02, buttonText01, buttonText02);
				buttonPress.gameObject.SetActive(true);
			};
			SetupButton(title, message, addListenerAction);
		}

		public virtual void SetThreeButton(DialogBoxMenuUI dialogBoxMenuUIBase, string title, string message, Action action01, Action action02, Action action03,
											string buttonText01 = default, string buttonText02 = default, string buttonText03 = default)
		{ }

		void SetupButton(string title, string message, Action addListenerAction)
		{
			titleTMP.text = title;
			descriptionTMP.text = message;

			addListenerAction?.Invoke();
		}

		Action SetupOnButtonPressedAction(DialogBoxMenuUI dialogBoxMenuUI, DialogBoxButtonPress buttonPress, Action action)
		{
			return () =>
			{
				action?.Invoke();

				// Disable the dialogBox and buttons.
				buttonPress.gameObject.SetActive(false);
				dialogBoxMenuUI.gameObject.SetActive(false);
				CancelAction = null;

				// Remove from stack.
				dialogBoxHandlerUI.DialogBoxStack.Pop();
			};
		}
	}
}
