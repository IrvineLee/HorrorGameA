using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Manager;

namespace Personal.InputProcessing
{
	public class DialogueCursorSet : GameInitialize
	{
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

		void OnDestroy()
		{
			standardUIMenuPanel.onOpen.RemoveAllListeners();
			standardUIMenuPanel.onClose.RemoveAllListeners();
		}
	}
}