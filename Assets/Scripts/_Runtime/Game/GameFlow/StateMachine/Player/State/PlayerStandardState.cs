using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.System.Handler;
using Personal.Constant;

namespace Personal.FSM.Character
{
	public class PlayerStandardState : ActorStateBase
	{
		protected Camera cam;

		PlayerStateMachine playerFSM;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			cam = StageManager.Instance.MainCamera;
			playerFSM = (PlayerStateMachine)actorStateMachine;
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			RaycastHit hit;

			float radius = ConstantFixed.PLAYER_LOOK_SPHERECAST_RADIUS;
			float length = ConstantFixed.PLAYER_LOOK_SPHERECAST_LENGTH;

			Vector3 startPos = cam.transform.position;
			Vector3 endPos = startPos + cam.transform.forward * length;

			if (!Input.GetMouseButtonDown(0)) return;

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

			interactable.HandleInteraction(stateMachine, () => playerFSM.SwitchToState(typeof(PlayerStandardState)).Forget()).Forget();
			playerFSM.SetState(null).Forget();
		}
	}
}