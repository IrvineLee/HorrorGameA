using UnityEngine;

using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Helper;
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
		StandardUISubtitlePanel[] subtitlePanelArray;
		UIButtonKeyTrigger uiButtonKeyTrigger;

		bool isChangeActionMap = true;

		protected override void Initialize()
		{
			var dialogueSystemController = GetComponentInChildren<DialogueSystemController>(true);
			GameObject dialogueUI = dialogueSystemController.displaySettings.dialogueUI;

			StandardDialogueUI standardDialogueUI = dialogueUI.GetComponentInChildren<StandardDialogueUI>(true);
			standardUIMenuPanel = standardDialogueUI.conversationUIElements.defaultMenuPanel;

			// Subtitle panels.
			subtitlePanelArray = ((StandardUIDialogueControls)standardDialogueUI.dialogueControls).subtitlePanels;

			dialogueResponseListHandler = GetComponentInChildren<DialogueResponseListHandler>(true);
			uiButtonKeyTrigger = GetComponentInChildren<UIButtonKeyTrigger>(true);

			var playerActionInput = InputManager.Instance.PlayerActionInput;
			InputDeviceManager.RegisterInputAction("Interact", playerActionInput.Player.Interact);

			InitializeResponseWindow();
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationStart(Transform actor)
		{
			UIManager.Instance.FooterIconDisplay.Begin(true);
			EnableSubtitlePanel(true);

			if (isChangeActionMap)
			{
				previousActionMap = InputManager.Instance.CurrentActionMapType;
				InputManager.Instance.EnableActionMap(actionMapType);
			}
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationEnd(Transform actor)
		{
			UIManager.Instance.FooterIconDisplay.Begin(false);
			EnableSubtitlePanel(false);

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
		/// <param name="isXInteract"></param>
		public void SwapInteractInput(bool isXInteract)
		{
			uiButtonKeyTrigger.buttonName = isXInteract ? interactStr : cancelStr;
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

		/// <summary>
		/// Initialize the response window.
		/// </summary>
		void InitializeResponseWindow()
		{
			// For the response window.
			standardUIMenuPanel.onOpen.AddListener(() =>
			{
				IsWaitingResponse = true;

				// Wait for the response to be selected first before setting mouse cursor.
				CoroutineHelper.WaitNextFrame(() =>
				{
					CursorManager.Instance.TrySetToMouseCursorForMouseControl(true);
				}, false, true);
			});

			standardUIMenuPanel.onClose.AddListener(() =>
			{
				IsWaitingResponse = false;
				CursorManager.Instance.TrySetToMouseCursorForMouseControl(false);
			});
		}

		/// <summary>
		/// Set active the subtitle panel.
		/// </summary>
		/// <param name="isFlag"></param>
		void EnableSubtitlePanel(bool isFlag)
		{
			// Somehow these must be started first so it get registered to "OnConversationLine" for the first text.
			foreach (var panel in subtitlePanelArray)
			{
				panel.gameObject.SetActive(isFlag);
				panel.subtitleText.gameObject.SetActive(isFlag);
			}
		}

		void OnApplicationQuit()
		{
			standardUIMenuPanel?.onOpen.RemoveAllListeners();
			standardUIMenuPanel?.onClose.RemoveAllListeners();

			InputDeviceManager.UnregisterInputAction("Interact");
		}
	}
}