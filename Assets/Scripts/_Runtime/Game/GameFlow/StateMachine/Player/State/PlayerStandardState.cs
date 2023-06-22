using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.System.Handler;
using Personal.Constant;
using Personal.InteractiveObject;
using Personal.Character;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : PlayerBaseState
	{
		protected Camera cam;

		float radius = ConstantFixed.PLAYER_LOOK_SPHERECAST_RADIUS;
		float length = ConstantFixed.PLAYER_LOOK_SPHERECAST_LENGTH;

		public override UniTask OnEnter()
		{
			base.OnEnter();

			cam = StageManager.Instance.MainCamera;
			return UniTask.CompletedTask;
		}

		public override void OnUpdate()
		{
			RaycastHit hit;

			Vector3 startPos = cam.transform.position;
			Vector3 endPos = startPos + cam.transform.forward * length;

			Debug.DrawLine(startPos, endPos, Color.green);
			if (Physics.SphereCast(startPos, radius, cam.transform.forward, out hit, length, 1 << (int)LayerType._Interactable))
			{
				HandleInteractable(hit);
				return;
			}

			CursorManager.Instance.SetToDefaultCrosshair();
		}

		public virtual void HandleInteractable(RaycastHit hit)
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