using UnityEngine;
using UnityEngine.UI;

using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Helper;
using Personal.Manager;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.Definition;
using UIButtonKeyTrigger = PixelCrushers.UIButtonKeyTrigger;

namespace Personal.Dialogue
{
	public class DialogueSetup : GameInitialize
	{
		[SerializeField] string interactStr = "Interact";
		[SerializeField] string cancelStr = "Cancel";

		public DialogueResponseListHandler DialogueResponseListHandler { get => dialogueResponseListHandler; }
		public bool IsWaitingResponse { get; private set; }

		public DisplaySettings.SubtitleSettings SubtitleSetting { get; private set; }
		public Button ContinueButton { get; private set; }

		StandardUIMenuPanel standardUIMenuPanel;
		StandardUISubtitlePanel[] subtitlePanelArray;
		UIButtonKeyTrigger uiButtonKeyTrigger;

		DialogueResponseListHandler dialogueResponseListHandler = null;
		ActionMapType previousActionMap;

		protected override void EarlyInitialize()
		{
			dialogueResponseListHandler = GetComponentInChildren<DialogueResponseListHandler>(true);
			uiButtonKeyTrigger = GetComponentInChildren<UIButtonKeyTrigger>(true);

			var dialogueSystemController = GetComponentInChildren<DialogueSystemController>(true);
			GameObject dialogueUI = dialogueSystemController.displaySettings.dialogueUI;

			StandardDialogueUI standardDialogueUI = dialogueUI.GetComponentInChildren<StandardDialogueUI>(true);
			standardUIMenuPanel = standardDialogueUI.conversationUIElements.defaultMenuPanel;

			InitMainPanel(standardDialogueUI);
			InitializeResponseWindow();

			// Continue button.
			SubtitleSetting = dialogueSystemController.displaySettings.subtitleSettings;
			ContinueButton = standardDialogueUI.conversationUIElements.defaultNPCSubtitlePanel.continueButton;

			// Subtitle panels.
			subtitlePanelArray = ((StandardUIDialogueControls)standardDialogueUI.dialogueControls).subtitlePanels;

			var playerActionInput = InputManager.Instance.PlayerActionInput;
			InputDeviceManager.RegisterInputAction("Interact", playerActionInput.Player.Interact);
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationStart(Transform actor)
		{
			UIManager.Instance.FooterIconDisplay.Begin(true);
			EnableSubtitlePanel(true);
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationEnd(Transform actor)
		{
			UIManager.Instance.FooterIconDisplay.Begin(false);
			EnableSubtitlePanel(false);

			IsWaitingResponse = false;

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
		/// TODO : This makes the dialogue go to the next dialogue after it ended. Normally used for monologue where the player has no authority over it.
		/// </summary>
		public void SetToAutoConversation()
		{

		}

		void InitMainPanel(StandardDialogueUI standardDialogueUI)
		{
			UIPanel mainPanel = standardDialogueUI.conversationUIElements.mainPanel;
			mainPanel.onOpen.AddListener(() =>
			{
				previousActionMap = InputManager.Instance.CurrentActionMapType;
				InputManager.Instance.EnableActionMap(ActionMapType.UI);
			});

			mainPanel.onClose.AddListener(() =>
			{
				if (previousActionMap == ActionMapType.None) return;
				InputManager.Instance?.EnableActionMap(previousActionMap);
			});
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
					CursorManager.Instance.HandleMouse();
				}, false, true);
			});

			standardUIMenuPanel.onClose.AddListener(() =>
			{
				IsWaitingResponse = false;
				CursorManager.Instance.HandleMouse();
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