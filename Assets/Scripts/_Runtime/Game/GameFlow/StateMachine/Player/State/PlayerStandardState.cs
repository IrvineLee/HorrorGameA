using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Character;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : PlayerBaseState
	{
		InteractableObject interactableObject;

		protected override void HandleOnInteractable(RaycastHit hit)
		{
			// All interactable objects collider should be at least 1 child deep into a gameobject.
			interactableObject = hit.transform.GetComponentInParent<InteractableObject>();

			if (!interactableObject) return;
			if (!interactableObject.enabled) return;

			CursorManager.Instance.SetCenterCrosshair(interactableObject.InteractCrosshairType);
			interactableObject.ShowOutline(true);

			if (!InputManager.Instance.GetButtonPush(InputManager.ButtonPush.Submit)) return;
			if (!interactableObject.enabled) return;

			Debug.Log("Hit interactable");
			playerFSM.SetLookAtTarget(interactableObject.ParentTrans.GetComponentInChildren<ActorController>()?.Head);

			CursorManager.Instance.SetCenterCrosshairToDefault();
			interactableObject.HandleInteraction(playerFSM).Forget();
		}

		protected override void HandleOffInteractable()
		{
			interactableObject?.ShowOutline(false);
		}
	}
}