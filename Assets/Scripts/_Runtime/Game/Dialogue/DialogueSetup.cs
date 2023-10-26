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
		public bool IsWaitingResponse { get; private set; }

		ActionMapType previousActionMap;

		StandardUIMenuPanel standardUIMenuPanel;
		UIButtonKeyTrigger uiButtonKeyTrigger;

		bool isChangeActionMap = true;

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
				IsWaitingResponse = true;
				HandleCursor();
				CursorManager.Instance.TrySetToMouseCursorForMouseControl(true);
			});

			standardUIMenuPanel.onClose.AddListener(() =>
			{
				IsWaitingResponse = false;
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
			if (isChangeActionMap)
			{
				previousActionMap = InputManager.Instance.CurrentActionMapType;
				InputManager.Instance.EnableActionMap(actionMapType);
			}

			HandleCursor();
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationEnd(Transform actor)
		{
			if (isChangeActionMap)
			{
				InputManager.Instance.EnableActionMap(previousActionMap);
			}

			IsWaitingResponse = false;
			isChangeActionMap = true;

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

		/// <summary>
		/// Disable input action change when starting conversation. It will automatically reset back to true after conversation ends.
		/// </summary>
		public void DisableInputActionChange()
		{
			isChangeActionMap = false;
		}

		/// <summary>
		/// TODO : This makes the dialogue go to the next dialogue after it ended. Normally used for monologue where the player has no authority over it.
		/// </summary>
		public void SetToAutoConversation()
		{

		}

		void HandleCursor()
		{
			if (!GameSceneManager.Instance.IsMainScene()) return;
			if (UIManager.Instance.ActiveInterfaceType != UI.UIInterfaceType.Dialogue) return;

			if (InputManager.Instance.IsCurrentDeviceMouse && IsWaitingResponse)
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