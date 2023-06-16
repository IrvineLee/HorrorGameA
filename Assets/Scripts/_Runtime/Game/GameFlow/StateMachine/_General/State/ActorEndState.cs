using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class ActorEndState : ActorStateBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			PoolManager.Instance.ReturnSpawnedObject(actorStateMachine.gameObject);
		}
	}
}