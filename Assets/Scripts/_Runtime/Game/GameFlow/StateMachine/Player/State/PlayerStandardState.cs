using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.System.Handler;
using Personal.Constant;
using Personal.Object;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : ActorStateBase
	{
		protected Camera cam;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			cam = StageManager.Instance.MainCamera;
		}

		public override void OnUpdate()
		{
			RaycastHit hit;

			float radius = ConstantFixed.PLAYER_LOOK_SPHERECAST_RADIUS;
			float length = ConstantFixed.PLAYER_LOOK_SPHERECAST_LENGTH;

			Vector3 startPos = cam.transform.position;
			Vector3 endPos = startPos + cam.transform.forward * length;

			if (!InputManager.Instance.IsInteract) return;

			if (Physics.SphereCast(startPos, radius, cam.transform.forward, out hit, length, 1 << (int)LayerType._Interactable))
			{
				HandleInteractable(hit);
			}

			Debug.DrawLine(startPos, endPos, Color.green);
		}

		public virtual void HandleInteractable(RaycastHit hit)
		{
			Debug.Log("Hit interactable");

			// All interactable objects collider should be at least 1 child deep into a gameobject.
			var interactable = hit.transform.GetComponentInParent<InteractableObject>();

			if (!interactable) return;
			if (!interactable.enabled) return;

			interactable.HandleInteraction(stateMachine, default).Forget();
		}
	}
}