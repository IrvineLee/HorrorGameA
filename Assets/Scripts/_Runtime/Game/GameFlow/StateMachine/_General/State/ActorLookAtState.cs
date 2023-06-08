using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character;

namespace Personal.FSM.Character
{
	public class ActorLookAtState : ActorStateBase
	{
		[SerializeField] bool isLookAtTarget = true;
		[SerializeField] Transform target = null;

		HeadLookAt headLookAt;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			headLookAt = actorStateMachine.HeadLookAt;

			if (target) headLookAt.SetTarget(target);
			headLookAt.SetLookAtTarget(isLookAtTarget);
		}
	}
}