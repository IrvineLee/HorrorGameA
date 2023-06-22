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

			CursorManager.Instance.SetCrosshair(interactable.InteractCrosshairType);

			if (!InputManager.Instance.IsInteract) return;
			if (!interactable.enabled) return;

			Debug.Log("Hit interactable");
			playerFSM.SetLookAtTarget(interactable.ParentTrans.GetComponentInChildren<ActorController>()?.Head);

			CursorManager.Instance.SetToDefaultCrosshair();
			interactable.HandleInteraction(playerFSM, default).Forget();
		}
	}
}