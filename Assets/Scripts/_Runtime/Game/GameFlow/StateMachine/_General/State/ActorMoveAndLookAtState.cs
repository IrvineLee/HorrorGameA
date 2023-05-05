using System;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Helper;

namespace Personal.FSM.General
{
	[Serializable]
	public class ActorMoveAndLookAtState : StateBase
	{
		[Tooltip("OnExit, body turn duration towards target")]
		[SerializeField] float exitBodyRotateDuration = 0.25f;

		protected ActorStateMachine actorStateMachine;
		protected NavMeshAgent navMeshAgent;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

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
			await base.OnUpdate();

			// The navmesh agent head animation should be looking at the target.
			await UniTask.DelayFrame(0);
		}

		/// <summary>
		/// Rotate the actor so it's facing the player.
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnExit()
		{
			await base.OnExit();

			Transform target = GetLookAtTarget();

			Vector3 direction = (target.position - navMeshAgent.transform.position).normalized;
			Quaternion endRotation = Quaternion.LookRotation(direction);

			CoroutineRun cr = CoroutineHelper.QuaternionLerpWithinSeconds(navMeshAgent.transform, navMeshAgent.transform.rotation, endRotation, exitBodyRotateDuration);
			while (!cr.IsDone)
			{
				await UniTask.DelayFrame(0);
			}
		}

		protected virtual Vector3 GetDestination() { return Vector3.zero; }
		protected virtual Transform GetLookAtTarget() { return StageManager.Instance.PlayerStateMachine.transform; }
	}
}