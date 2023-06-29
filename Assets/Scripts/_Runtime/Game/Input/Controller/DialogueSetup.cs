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

		protected override void Initialize()
		{
			var dialogueSystemController = GetComponentInChildren<DialogueSystemController>(true);
			GameObject dialogueUI = dialogueSystemController.displaySettings.dialogueUI;
			standardUIMenuPanel = dialogueUI.GetComponentInChildren<StandardDialogueUI>(true).conversationUIElements.defaultMenuPanel;

			// For the response window.
			standardUIMenuPanel.onOpen.AddListener(() => CursorManager.Instance.SetToMouseCursor(true));
			standardUIMenuPanel.onClose.AddListener(() => CursorManager.Instance.SetToMouseCursor(false));
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
		}

		void OnDestroy()
		{
			standardUIMenuPanel.onOpen.RemoveAllListeners();
			standardUIMenuPanel.onClose.RemoveAllListeners();
		}
	}
}