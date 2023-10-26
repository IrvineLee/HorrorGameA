using UnityEngine;

using PixelCrushers.DialogueSystem;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class DialogueTriggerHandler : InteractableObject
	{
		DialogueSystemTrigger dialogueSystemTrigger;

		protected override void Initialize()
		{
			base.Initialize();
			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
		}

		protected void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;
			if (!dialogueSystemTrigger) return;

			StageManager.Instance.DialogueController.DialogueSetup.DisableInputActionChange();
			dialogueSystemTrigger?.OnUse(other.transform);

			// Disable the trigger collider.
			currentCollider.enabled = false;
		}
	}
}

