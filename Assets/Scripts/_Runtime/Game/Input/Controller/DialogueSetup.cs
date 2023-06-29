using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.Manager;
using Personal.GameState;

namespace Personal.InputProcessing
{
	public class DialogueSetup : GameInitialize
	{
		[SerializeField] ActionMapType actionMapType = ActionMapType.BasicControl;

		ActionMapType previousActionMap;

		StandardUIMenuPanel standardUIMenuPanel;
		bool isWaitingResponse;

		protected override void Initialize()
		{
			var dialogueSystemController = GetComponentInChildren<DialogueSystemController>(true);
			GameObject dialogueUI = dialogueSystemController.displaySettings.dialogueUI;
			standardUIMenuPanel = dialogueUI.GetComponentInChildren<StandardDialogueUI>(true).conversationUIElements.defaultMenuPanel;

			// For the response window.
			standardUIMenuPanel.onOpen.AddListener(() =>
			{
				isWaitingResponse = true;

				if (InputManager.Instance.InputDeviceType == InputDeviceType.Gamepad) return;
				CursorManager.Instance.SetToMouseCursor(true);
			});

			standardUIMenuPanel.onClose.AddListener(() =>
			{
				isWaitingResponse = false;

				if (InputManager.Instance.InputDeviceType == InputDeviceType.Gamepad) return;
				CursorManager.Instance.SetToMouseCursor(false);
			});

			InputManager.Instance.OnDeviceIconChanged += HandleCursor;
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationStart(Transform actor)
		{
			previousActionMap = InputManager.Instance.CurrentActionMapType;
			InputManager.Instance.EnableActionMap(actionMapType);
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationEnd(Transform actor)
		{
			InputManager.Instance.EnableActionMap(previousActionMap);
			isWaitingResponse = false;
		}

		void HandleCursor()
		{
			if (InputManager.Instance.InputDeviceType == InputDeviceType.KeyboardMouse && isWaitingResponse)
			{
				CursorManager.Instance.SetToMouseCursor(true);
				return;
			}

			CursorManager.Instance.SetToMouseCursor(false);
		}

		void OnDestroy()
		{
			standardUIMenuPanel.onOpen.RemoveAllListeners();
			standardUIMenuPanel.onClose.RemoveAllListeners();

			InputManager.Instance.OnDeviceIconChanged -= HandleCursor;
		}
	}
}