using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Character;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : PlayerBaseState
	{
		protected override void HandleInteractable(RaycastHit hit)
		{
			// All interactable objects collider should be at least 1 child deep into a gameobject.
			var interactable = hit.transform.GetComponentInParent<InteractableObject>();

			if (!interactable) return;
			if (!interactable.enabled) return;

			CursorManager.Instance.SetCenterCrosshair(interactable.InteractCrosshairType);

			if (!InputManager.Instance.IsInteract) return;
			if (!interactable.enabled) return;

			Debug.Log("Hit interactable");
			playerFSM.SetLookAtTarget(interactable.ParentTrans.GetComponentInChildren<ActorController>()?.Head);

			CursorManager.Instance.SetToDefaultCenterCrosshair();
			interactable.HandleInteraction(playerFSM, default).Forget();
		}
	}
}