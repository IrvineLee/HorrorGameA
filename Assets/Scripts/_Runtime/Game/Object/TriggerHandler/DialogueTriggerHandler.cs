using UnityEngine;

namespace Personal.InteractiveObject
{
	public class DialogueTriggerHandler : InteractableObject
	{
		protected override void Initialize()
		{
			base.Initialize();
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

