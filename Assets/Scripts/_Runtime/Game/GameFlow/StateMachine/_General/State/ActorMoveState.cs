using System;
using UnityEngine;
using UnityEngine.AI;

using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Personal.GameState;
using Helper;

namespace Personal.FSM.Character
{
	public class ActorMoveState : ActorStateBase
	{
		[Tooltip("OnExit, body turn duration towards target")]
		[SerializeField] float exitBodyRotateDuration = 0.25f;

		[SerializeField] TargetInfo.TargetType targetType = TargetInfo.TargetType.MoveTo;

		[ShowIf("@targetType == Personal.GameState.TargetInfo.TargetType.Player")]
		[SerializeField] float distanceBetweenActor = 0;

		protected NavMeshAgent navMeshAgent;
		protected Transform target;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			RunActorAnimation();

			target = GetTarget();
			navMeshAgent = actorStateMachine.NavMeshAgent;
			navMeshAgent.isStopped = false;
			navMeshAgent.destination = target.position;

			await UniTask.WaitUntil(() => navMeshAgent && navMeshAgent.remainingDistance <= distanceBetweenActor, cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		public override void OnUpdate()
		{
			navMeshAgent.destination = target.position;
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			navMeshAgent.isStopped = true;

			// Rotate the actor so it's facing the target.
			Transform target = GetLookAtTarget();

			Vector3 direction = navMeshAgent.transform.position.GetNormalizedDirectionTo(target.position);
			Quaternion endRotation = Quaternion.LookRotation(direction);

			Transform actorController = actorStateMachine.ActorController.transform;
			CoroutineHelper.QuaternionLerpWithinSeconds(actorController, actorController.rotation, endRotation, exitBodyRotateDuration);
		}

		protected virtual Transform GetTarget()
		{
			OrderedStateMachine orderedStateMachine = (OrderedStateMachine)actorStateMachine;

			Transform target = orderedStateMachine.TargetInfo.SpawnAtFirst;

			if (targetType == TargetInfo.TargetType.MoveTo)
			{
				target = orderedStateMachine.TargetInfo.MoveToFirst;
			}
			else if (targetType == TargetInfo.TargetType.Leave)
			{
				target = orderedStateMachine.TargetInfo.LeaveAtFirst;
			}
			else if (targetType == TargetInfo.TargetType.Player)
			{
				target = orderedStateMachine.TargetInfo.Player;
			}

			return target;
		}

		protected virtual Transform GetLookAtTarget() { return null; }
	}
}