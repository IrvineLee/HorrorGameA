using System;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.GameState;
using Helper;

namespace Personal.FSM.Character
{
	[Serializable]
	public class ActorMoveState : ActorStateBase
	{
		[Tooltip("OnExit, body turn duration towards target")]
		[SerializeField] float exitBodyRotateDuration = 0.25f;

		[SerializeField] TargetInfo.TargetType targetType = TargetInfo.TargetType.MoveTo;

		protected NavMeshAgent navMeshAgent;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			navMeshAgent = actorStateMachine.NavMeshAgent;

			RunActorAnimation();
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

		protected virtual Vector3 GetDestination()
		{
			Transform target = actorStateMachine.TargetInfo.SpawnAtFirst;

			if (targetType == TargetInfo.TargetType.MoveTo)
			{
				target = actorStateMachine.TargetInfo.MoveToFirst;
			}
			else if (targetType == TargetInfo.TargetType.Leave)
			{
				target = actorStateMachine.TargetInfo.MoveToLast;
			}

			return target.position;
		}
		protected virtual Transform GetLookAtTarget() { return StageManager.Instance.PlayerFSM.transform; }
	}
}