using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class DialogueTriggerHandlerOnce : TriggerHandlerOnce
	{
		protected override void OnTriggerEnter(Collider other)
		{
			if (!IsTriggerable()) return;

			StageManager.Instance.DialogueController.DialogueSetup.DisableInputActionChange();

			DialogueSystemTrigger dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			if (!dialogueSystemTrigger) return;

			dialogueSystemTrigger?.OnUse(other.transform);

			// Disable the trigger collider.
			currentCollider.enabled = false;
		}
	}
}

