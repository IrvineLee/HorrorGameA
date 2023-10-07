using UnityEngine;
using UnityEngine.EventSystems;

using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Personal.Manager;
using Personal.InputProcessing;
using Personal.GameState;
using UIButtonKeyTrigger = PixelCrushers.UIButtonKeyTrigger;

namespace Personal.Dialogue
{
	public class DialogueSetup : GameInitialize
	{
		[SerializeField] ActionMapType actionMapType = ActionMapType.UI;
		[SerializeField] string interactStr = "Interact";
		[SerializeField] string cancelStr = "Cancel";

		[SerializeField] DialogueResponseListHandler dialogueResponseListHandler = null;

		public DialogueResponseListHandler DialogueResponseListHandler { get => dialogueResponseListHandler; }
		public bool IsWaitingResponse { get => isWaitingResponse; }

		ActionMapType previousActionMap;

		StandardUIMenuPanel standardUIMenuPanel;
		bool isWaitingResponse;

		UIButtonKeyTrigger uiButtonKeyTrigger;

		protected override void Initialize()
		{
			var dialogueSystemController = GetComponentInChildren<DialogueSystemController>(true);

			GameObject dialogueUI = dialogueSystemController.displaySettings.dialogueUI;
			standardUIMenuPanel = dialogueUI.GetComponentInChildren<StandardDialogueUI>(true).conversationUIElements.defaultMenuPanel;

			dialogueResponseListHandler = GetComponentInChildren<DialogueResponseListHandler>(true);

			uiButtonKeyTrigger = GetComponentInChildren<UIButtonKeyTrigger>(true);

			var playerActionInput = InputManager.Instance.PlayerActionInput;
			InputDeviceManager.RegisterInputAction("Interact", playerActionInput.Player.Interact);

			// For the response window.
			standardUIMenuPanel.onOpen.AddListener(() =>
			{
				isWaitingResponse = true;
				HandleCursor();
				CursorManager.Instance.TrySetToMouseCursorForMouseControl(true);
			});

			standardUIMenuPanel.onClose.AddListener(() =>
			{
				isWaitingResponse = false;
				CursorManager.Instance.TrySetToMouseCursorForMouseControl(false);
			});

			ResponseFocus(true);
			InputManager.OnDeviceIconChanged += HandleCursor;
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationStart(Transform actor)
		{
			previousActionMap = InputManager.Instance.CurrentActionMapType;
			InputManager.Instance.EnableActionMap(actionMapType);

			HandleCursor();
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

		/// <summary>
		/// Swap interact input.
		/// </summary>
		/// <param name="isUSInteract"></param>
		public void SwapInteractInput(bool isUSInteract)
		{
			uiButtonKeyTrigger.buttonName = isUSInteract ? interactStr : cancelStr;
		}

		void HandleCursor()
		{
			if (!GameSceneManager.Instance.IsMainScene()) return;
			if (UIManager.Instance.ActiveInterfaceType != UI.UIInterfaceType.Dialogue) return;

			if (InputManager.Instance.IsCurrentDeviceMouse && isWaitingResponse)
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
			standardUIMenuPanel.focusCheckFrequency = isFlag ? 0.1f : 0;
		}

		void OnApplicationQuit()
		{
			standardUIMenuPanel?.onOpen.RemoveAllListeners();
			standardUIMenuPanel?.onClose.RemoveAllListeners();

			InputManager.OnDeviceIconChanged -= HandleCursor;
			InputDeviceManager.UnregisterInputAction("Interact");
		}
	}
}