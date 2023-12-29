using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.InteractiveObject;
using Personal.Character;
using Personal.Definition;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : PlayerBaseState
	{
		InteractableObject previousInteractable;
		LookAtInfo lookAtInfo = new LookAtInfo();

		protected override void HandleOnInteractable(RaycastHit hit)
		{
			// All interactable objects collider should be at least 1 child deep into a gameobject.
			InteractableObject interactable = hit.transform.GetComponentInParent<InteractableObject>();

			// You are hitting another object, so off the other object interaction.
			if (previousInteractable && interactable &&
				interactable.gameObject != previousInteractable.gameObject)
			{
				HandleOffInteractable();
			}

			if (!interactable) return;
			if (!interactable.IsInteractable) return;

			CursorManager.Instance.SetCenterCrosshair(interactable.InteractCrosshairType);
			interactable.ShowOutline(true);

			previousInteractable = interactable;

			if (!InputManager.Instance.GetButtonPush(InputManager.ButtonPush.Submit)) return;

			Debug.Log("Hit interactable");
			var hitActorController = interactable.GetComponentInChildren<ActorController>();
			if (hitActorController)
			{
				lookAtInfo.LookAt = hitActorController.Head;
				playerFSM.SetLookAtInfo(lookAtInfo);
			}

			CursorManager.Instance.SetCenterCrosshair(CursorDefinition.CrosshairType.FPS);

			ifsmHandler.OnBegin(typeof(PlayerIdleState));
			interactable.HandleInteraction(playerFSM, ifsmHandler.OnExit).Forget();
		}

		protected override void HandleOffInteractable()
		{
			previousInteractable?.ShowOutline(false);
			previousInteractable = null;
		}
	}
}