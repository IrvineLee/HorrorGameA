using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.Interface;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_IProcessDialogue : InteractionEnd
	{
		[SerializeField] Transform iProcessTrans = null;

		DialogueSystemTrigger dialogueSystemTrigger;

		protected override void Initialize()
		{
			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
		}

		protected override bool IsEnded()
		{
			IProcess iProcess = iProcessTrans.GetComponentInChildren<IProcess>(true);
			if (iProcess == null) return false;
			if (!iProcess.IsCompleted()) return false;

			return true;
		}

		protected override async void HandleInteractable()
		{
			if (!dialogueSystemTrigger) return;

			dialogueSystemTrigger.OnUse();
			await StageManager.Instance.DialogueController.WaitDialogueEnd();
		}
	}
}

