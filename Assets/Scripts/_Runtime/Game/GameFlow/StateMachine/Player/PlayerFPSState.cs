using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class PlayerFPSState : ActorBase
	{
		[SerializeField] ParentMoveFollowChild parentMoveFollowChild = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			//parentMoveFollowChild = 
		}

		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();

			HandleMoveParent();
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

		void HandleMoveParent()
		{
			if (!parentMoveFollowChild.ChildTarget) return;

			if (parentMoveFollowChild.IsFollowPosition)
			{
				parentMoveFollowChild.ParentTarget.position = parentMoveFollowChild.ChildTarget.position + parentMoveFollowChild.PositionOffset;
				parentMoveFollowChild.ChildTarget.localPosition = Vector3.zero;
			}

			if (parentMoveFollowChild.IsFollowRotation)
			{
				parentMoveFollowChild.ParentTarget.rotation = parentMoveFollowChild.ChildTarget.rotation;
				parentMoveFollowChild.ChildTarget.localRotation = Quaternion.identity;
			}
		}
	}
}