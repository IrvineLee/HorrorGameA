using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.Manager;
using Personal.GameState;
using PixelCrushers;
using UnityEngine.EventSystems;

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
				HandleCursor();

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
			InputDeviceManager.instance.alwaysAutoFocus = false;
		}

		void HandleCursor()
		{
			if (InputManager.Instance.InputDeviceType == InputDeviceType.KeyboardMouse && isWaitingResponse)
			{
				ResponseFocus(false);
				EventSystem.current.SetSelectedGameObject(null);

				return;
			}
			ResponseFocus(true);
		}

		void ResponseFocus(bool isFlag)
		{
			InputDeviceManager.instance.alwaysAutoFocus = isFlag;
			CursorManager.Instance.SetToMouseCursor(!isFlag);

			standardUIMenuPanel.focusCheckFrequency = isFlag ? 0.1f : 0;
		}

		void OnApplicationQuit()
		{
			standardUIMenuPanel.onOpen.RemoveAllListeners();
			standardUIMenuPanel.onClose.RemoveAllListeners();

			InputManager.Instance.OnDeviceIconChanged -= HandleCursor;
		}
	}
}