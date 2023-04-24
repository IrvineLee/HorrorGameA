using UnityEngine;

using Cysharp.Threading.Tasks;
using System;
using UnityEngine.AI;
using Personal.Manager;
using Helper;

namespace Personal.FSM
{
	[Serializable]
	public class ActorMoveState : StateBase
	{
		public ActorMoveState(StateMachineBase stateMachine) : base(stateMachine) { }

		protected ActorStateMachine actorStateMachine;
		protected NavMeshAgent navMeshAgent;

		public override async UniTask OnEnter()
		{
			Debug.Log("CustomerComeCounter");

			actorStateMachine = ((ActorStateMachine)stateMachine);
			navMeshAgent = actorStateMachine.NavMeshAgent;

			navMeshAgent.destination = GetDestination();
			await UniTask.WaitUntil(() => navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0);

			return;
		}

		/// <summary>
		/// Move to the target.
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnUpdate()
		{
			navMeshAgent.destination = GetDestination();
			await UniTask.DelayFrame(0);
		}

		/// <summary>
		/// Rotate the actor so it's looking at the player.
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnExit()
		{
			Transform target = GetLookAtTarget();

			Vector3 direction = (target.position - navMeshAgent.transform.position).normalized;
			Quaternion endRotation = Quaternion.LookRotation(direction);

			CoroutineRun cr = CoroutineHelper.QuaternionLerpWithinSeconds(navMeshAgent.transform, navMeshAgent.transform.rotation, endRotation, 0.25f);
			while (!cr.IsDone)
			{
				await UniTask.DelayFrame(1);
			}
		}

		protected virtual Vector3 GetDestination() { return Vector3.zero; }
		protected virtual Transform GetLookAtTarget() { return StageManager.Instance.PlayerController.transform; }
	}
}