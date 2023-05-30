using System;
using UnityEngine;

using TMPro;
using static Personal.UI.Dialog.DialogBoxEnum;
using Cysharp.Threading.Tasks;

namespace Personal.UI.Dialog
{
	public class DialogBoxMenuUI : DialogBoxMenuUIBase
	{
		[SerializeField] DialogDisplayType dialogType = DialogDisplayType.DialogButtonConfirmationBox;

		public DialogDisplayType DialogType { get => dialogType; }
		public Action CancelAction { get; private set; }

		[SerializeField] TextMeshProUGUI titleTMP = null;
		[SerializeField] TextMeshProUGUI descriptionTMP = null;

		RectTransform rectTransform;

		public async override UniTask Initialize()
		{
			await base.Initialize();

			rectTransform = GetComponentInChildren<RectTransform>();
		}

		public void SetSize(Vector2 size)
		{
			rectTransform.sizeDelta = size;
		}

		public virtual void SetOneButtonOk(DialogBoxButtonPress buttonPress, string title, string message, Action action, string buttonText = default)
		{
			Action addListenerAction = () =>
			{
				Action onPressed = SetupOnButtonPressedAction(transform, action);
				CancelAction = onPressed;

				// Enable the correct buttons.
				buttonPress.AddListenerToButtonOnce(onPressed, buttonText);
			};
			SetupButton(title, message, addListenerAction);
		}

		public virtual void SetTwoButtonYesNo(DialogBoxButtonPress buttonPress, string title, string message, Action action01, Action action02,
											   string buttonText01 = default, string buttonText02 = default)
		{
			Action addListenerAction = () =>
			{
				Action onPressed01 = SetupOnButtonPressedAction(transform, action01);
				Action onPressed02 = SetupOnButtonPressedAction(transform, action02);
				CancelAction = onPressed02;

				// Enable the correct buttons.
				buttonPress.AddListenerToButtonOnce(onPressed01, onPressed02, buttonText01, buttonText02);
			};
			SetupButton(title, message, addListenerAction);
		}

		public virtual void SetThreeButton(DialogBoxButtonPress buttonPress, string title, string message, Action action01, Action action02, Action action03,
											string buttonText01 = default, string buttonText02 = default, string buttonText03 = default)
		{ }

		void SetupButton(string title, string message, Action addListenerAction)
		{
			titleTMP.text = title;
			descriptionTMP.text = message;

			addListenerAction?.Invoke();
		}

		Action SetupOnButtonPressedAction(Transform dialogTrans, Action action)
		{
			return () =>
			{
				action?.Invoke();

				// Disable the dialogBox and remove from stack.
				dialogTrans.gameObject.SetActive(false);
				dialogBoxHandlerUI.DialogBoxStack.Pop();
			};
		}
	}
}
