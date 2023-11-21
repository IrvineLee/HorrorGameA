using UnityEngine;

using PixelCrushers.DialogueSystem;

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

			// TODO: This is a trigger where the player still can move while the dialogue is happening...
			dialogueSystemTrigger?.OnUse(other.transform);

			colliderTrans.gameObject.SetActive(false);
		}
	}
}

