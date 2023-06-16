using System;
using UnityEngine;

using TMPro;
using static Personal.UI.Window.WindowEnum;
using Personal.Manager;

namespace Personal.UI.Window
{
	public class WindowMenuUI : WindowMenuUIBase
	{
		[SerializeField] WindowDisplayType windowType = WindowDisplayType.ButtonConfirmationBox;

		public WindowDisplayType WindowType { get => windowType; }
		public Action CancelAction { get; private set; }

		[SerializeField] TextMeshProUGUI titleTMP = null;
		[SerializeField] TextMeshProUGUI descriptionTMP = null;

		RectTransform rectTransform;

		public override void InitialSetup()
		{
			base.InitialSetup();

			rectTransform = GetComponentInChildren<RectTransform>();
		}

		public override void CloseWindow()
		{
			CancelAction?.Invoke();
		}

		public void SetSize(Vector2 size)
		{
			rectTransform.sizeDelta = size;
		}

		/// <summary>
		/// The last action is always the cancel action.
		/// </summary>
		public virtual void SetOneButtonOk(WindowButtonPress buttonPress, string title, string message, Action action, string buttonText = default)
		{
			Action addListenerAction = () =>
			{
				Action onPressed = AddDisableWindowOnClick(transform, action);
				CancelAction = onPressed;

				// Enable the correct buttons.
				buttonPress.AddListenerToButtonOnce(onPressed, buttonText);
			};
			SetupButton(title, message, addListenerAction);
		}

		/// <summary>
		/// The last action is always the cancel action.
		/// </summary>
		public virtual void SetTwoButtonYesNo(WindowButtonPress buttonPress, string title, string message, Action action01, Action action02,
											   string buttonText01 = default, string buttonText02 = default)
		{
			Action addListenerAction = () =>
			{
				Action onPressed01 = AddDisableWindowOnClick(transform, action01);
				Action onPressed02 = AddDisableWindowOnClick(transform, action02);
				CancelAction = onPressed02;

				// Enable the correct buttons.
				buttonPress.AddListenerToButtonOnce(onPressed01, onPressed02, buttonText01, buttonText02);
			};
			SetupButton(title, message, addListenerAction);
		}

		/// <summary>
		/// The last action is always the cancel action.
		/// </summary>
		public virtual void SetThreeButton(WindowButtonPress buttonPress, string title, string message, Action action01, Action action02, Action action03,
											string buttonText01 = default, string buttonText02 = default, string buttonText03 = default)
		{ }

		void SetupButton(string title, string message, Action addListenerAction)
		{
			titleTMP.text = title;
			descriptionTMP.text = message;

			addListenerAction?.Invoke();
		}

		Action AddDisableWindowOnClick(Transform windowTrans, Action action)
		{
			return () =>
			{
				action?.Invoke();

				// Disable the window and remove from stack.
				windowTrans.gameObject.SetActive(false);
				UIManager.Instance.WindowStack.Pop();
			};
		}
	}
}
