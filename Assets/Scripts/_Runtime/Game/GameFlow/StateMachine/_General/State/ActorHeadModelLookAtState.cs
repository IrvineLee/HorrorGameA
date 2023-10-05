using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.NPC;

namespace Personal.FSM.Character
{
	public class ActorHeadModelLookAtState : ActorStateBase
	{
		[SerializeField] bool isLookAtTarget = true;
		[SerializeField] Transform target = null;

		HeadModelLookAt headModelLookAt;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			headModelLookAt = actorStateMachine.HeadModelLookAt;

			// All headModelLookAt default target is the MainCameara.
			if (target) headModelLookAt.SetTarget(target);
			headModelLookAt.SetLookAtTarget(isLookAtTarget);
		}
	}
}