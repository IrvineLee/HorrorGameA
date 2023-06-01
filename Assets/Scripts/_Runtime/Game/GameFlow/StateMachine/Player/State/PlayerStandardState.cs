using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.System.Handler;
using Personal.Constant;
using Personal.Object;

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
				OnHitInteractable(hit);
			}

			Debug.DrawLine(startPos, endPos, Color.green);
		}

		public virtual void OnHitInteractable(RaycastHit hit)
		{
			Debug.Log("Hit interactable");

			var interactable = hit.transform.GetComponentInChildren<InteractableObject>();

			if (!interactable) return;
			if (!interactable.enabled) return;

			interactable.HandleInteraction(stateMachine, default).Forget();
		}
	}
}