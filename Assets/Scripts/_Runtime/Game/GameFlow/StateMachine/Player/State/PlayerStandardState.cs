using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Character;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : PlayerBaseState
	{
		protected override void HandleOnInteractable(RaycastHit hit)
		{
			// All interactable objects collider should be at least 1 child deep into a gameobject.
			var interactable = hit.transform.GetComponentInParent<InteractableObject>();

			if (!interactable) return;
			if (!interactable.enabled) return;

			CursorManager.Instance.SetCenterCrosshair(interactable.InteractCrosshairType);
			interactable.ShowOutline(true);

			if (!InputManager.Instance.GetButtonPush(InputManager.ButtonPush.Submit)) return;
			if (!interactable.enabled) return;

			Debug.Log("Hit interactable");
			playerFSM.SetLookAtTarget(interactable.ParentTrans.GetComponentInChildren<ActorController>()?.Head);

			CursorManager.Instance.SetCenterCrosshairToDefault();
			interactable.HandleInteraction(playerFSM).Forget();
		}

		protected override void HandleOffInteractable(Transform previousHitTrans)
		{
			var interactable = previousHitTrans.GetComponentInParent<InteractableObject>();
			interactable.ShowOutline(false);
		}
	}
}