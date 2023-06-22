using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.NPC;

namespace Personal.FSM.Character
{
	public class ActorLookAtState : ActorStateBase
	{
		[SerializeField] bool isLookAtTarget = true;
		[SerializeField] Transform target = null;

		HeadModelLookAt headModelLookAt;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			headModelLookAt = actorStateMachine.HeadModelLookAt;

			if (target) headModelLookAt.SetTarget(target);
			headModelLookAt.SetLookAtTarget(isLookAtTarget);
		}
	}
}