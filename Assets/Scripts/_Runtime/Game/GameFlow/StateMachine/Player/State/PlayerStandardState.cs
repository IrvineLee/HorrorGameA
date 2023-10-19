using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Character;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : PlayerBaseState
	{
		InteractableObject previousInteractable;

		protected override void HandleOnInteractable(RaycastHit hit)
		{
			// You are hitting another object, so off the other object interaction.
			if (previousInteractable && hit.transform.gameObject != previousInteractable.gameObject)
			{
				HandleOffInteractable();
			}

			// All interactable objects collider should be at least 1 child deep into a gameobject.
			InteractableObject interactable = hit.transform.GetComponentInParent<InteractableObject>();

			if (!interactable) return;
			if (!interactable.IsInteractable) return;

			CursorManager.Instance.SetCenterCrosshair(interactable.InteractCrosshairType);
			interactable.ShowOutline(true);

			previousInteractable = interactable;

			if (!InputManager.Instance.GetButtonPush(InputManager.ButtonPush.Submit)) return;

			Debug.Log("Hit interactable");
			playerFSM.SetLookAtTarget(interactable.ParentTrans.GetComponentInChildren<ActorController>()?.Head);

			CursorManager.Instance.SetCenterCrosshairToDefault();
			interactable.HandleInteraction(playerFSM).Forget();
		}

		protected override void HandleOffInteractable()
		{
			previousInteractable?.ShowOutline(false);
			previousInteractable = null;
		}
	}
}