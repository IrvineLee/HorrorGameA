using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.System.Handler;
using Personal.Constant;

namespace Personal.FSM.Character
{
	public class PlayerDefaultState : ActorStateBase
	{
		protected Camera cam;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			cam = StageManager.Instance.MainCamera;
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			RaycastHit hit;

			float radius = ConstantFixed.PLAYER_LOOK_SPHERECAST_RADIUS;
			float length = ConstantFixed.PLAYER_LOOK_SPHERECAST_LENGTH;

			Vector3 startPos = cam.transform.position;
			Vector3 endPos = startPos + cam.transform.forward * length;

			if (Physics.SphereCast(startPos, radius, cam.transform.forward, out hit, length, 1 << (int)LayerType._Pickupable))
			{
				OnHitPickupable(hit);
			}

			Debug.DrawLine(startPos, endPos, Color.green);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
		}

		public virtual void OnHitPickupable(RaycastHit hit)
		{
			// TODO: Item enter into inventory.
			Debug.Log(hit.transform.name);
		}
	}
}