using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using Helper;

namespace Personal.FSM.Character
{
	public class ActorMoveState : ActorStateBase
	{
		[Tooltip("Distance between the actors")]
		[SerializeField] protected float distanceBetweenActor = 0;

		[Tooltip("Body turn towards target speed")]
		[SerializeField] protected float turnTowardsSpeed = 5f;

		public bool IsReached { get; private set; }

		protected NavMeshAgent navMeshAgent;
		protected Transform moveToTarget;

		protected bool isNavMeshEnabled;
		protected Quaternion endRotation;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			RunActorAnimation();

			moveToTarget = GetTarget();

			navMeshAgent = actorStateMachine.NavMeshAgent;

			isNavMeshEnabled = navMeshAgent.enabled;
			navMeshAgent.enabled = true;
			navMeshAgent.isStopped = false;
			navMeshAgent.destination = moveToTarget.position;

			IsReached = false;

			await UniTask.WaitUntil(() => navMeshAgent && navMeshAgent.remainingDistance <= distanceBetweenActor, cancellationToken: this.GetCancellationTokenOnDestroy());

			IsReached = true;
			transform.rotation = endRotation;

			navMeshAgent.enabled = isNavMeshEnabled;
		}

		public override void OnUpdate()
		{
			if (IsReached) return;

			navMeshAgent.destination = moveToTarget.position;
			TurnTowardsTarget();
		}

		protected virtual Transform GetTarget() { return null; }

		protected virtual Transform GetTurnTowardsTarget() { return null; }

		void TurnTowardsTarget()
		{
			// Rotate the actor so it's facing the target.
			Transform target = GetTurnTowardsTarget();

			Vector3 direction = navMeshAgent.transform.position.GetNormalizedDirectionTo(target.position);
			endRotation = Quaternion.LookRotation(direction);

			Transform actorController = actorStateMachine.ActorController.transform;
			transform.rotation = Quaternion.Slerp(actorController.rotation, endRotation, Time.deltaTime * turnTowardsSpeed);
		}
	}
}