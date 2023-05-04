using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.General
{
	public class ActorEndState : StateBase
	{
		ActorStateMachine actorStateMachine;

		public override async UniTask OnEnter()
		{
			Debug.Log("Actor end state");

			actorStateMachine = (ActorStateMachine)stateMachine;
			PoolManager.Instance.ReturnSpawnedActor(actorStateMachine.NavMeshAgent.gameObject);

			await UniTask.DelayFrame(1);
			return;
		}

		public override async UniTask OnUpdate()
		{
			await UniTask.DelayFrame(0);
		}

		public override async UniTask OnExit()
		{
			await UniTask.DelayFrame(0);
		}
	}
}