using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers;

namespace Personal.FSM.Character
{
	public class PlayerFPSState : ActorBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			//RaycastHit hit;

			//Vector3 p1 = transform.position + charCtrl.center;
			//float distanceToObstacle = 0;

			//// Cast a sphere wrapping character controller 10 meters forward
			//// to see if it is about to hit anything.
			//if (Physics.SphereCast(p1, charCtrl.height / 2, transform.forward, out hit, 10))
			//{
			//    distanceToObstacle = hit.distance;
			//}
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
		}
	}
}