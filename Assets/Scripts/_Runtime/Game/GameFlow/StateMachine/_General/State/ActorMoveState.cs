using UnityEngine;
using UnityEngine.AI;

using Helper;
using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	/// <summary>
	/// Move towards the target while turning to look at it.
	/// Upon reaching, make the final rotation towards target to ensure it's always ends at the same value.
	/// Does not affect the camera, only the body.
	/// </summary>
	public class ActorMoveState : ActorStateBase
	{
		[Tooltip("Distance between the actors")]
		[SerializeField] protected float distanceBetweenActor = 0;

		[Tooltip("The speed of turning when walking towards target")]
		[SerializeField] protected float updateTurnTowardsSpeed = 2f;

		[Tooltip("Upon reaching the target, body turn towards target duration")]
		[SerializeField] protected float endTurnTowardsDuration = 0.5f;

		public bool IsReached { get; private set; }

		protected NavMeshAgent navMeshAgent;
		protected Transform moveToTarget;
		protected Transform turnToTarget;

		protected bool isNavMeshEnabled;

		Quaternion startQuaternion;
		Quaternion endQuaternion;
		float timer;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			navMeshAgent = actorStateMachine.NavMeshAgent;
			if (!navMeshAgent) return;

			RunActorAnimation();
			Initialize();

			// You need to wait for the nav mesh to calculate the remaining distance first.
			await UniTask.NextFrame();

			IsReached = false;
			await UniTask.WaitUntil(() => IsReached, cancellationToken: this.GetCancellationTokenOnDestroy());

			navMeshAgent.enabled = isNavMeshEnabled;
		}

		public override void OnUpdate()
		{
			if (IsReached) return;

			navMeshAgent.destination = moveToTarget.position;
			TurnTowardsTargetByUpdate();
		}

		protected virtual Transform GetTarget() { return null; }

		protected virtual Transform GetLookAtTarget() { return null; }

		void Initialize()
		{
			moveToTarget = GetTarget();
			turnToTarget = GetLookAtTarget();

			isNavMeshEnabled = navMeshAgent.enabled;
			navMeshAgent.enabled = true;
			navMeshAgent.isStopped = false;
			navMeshAgent.destination = moveToTarget.position;

			timer = 0;
		}

		void TurnTowardsTargetByUpdate()
		{
			Transform actorController = actorStateMachine.ActorController.transform;

			if (navMeshAgent.remainingDistance > distanceBetweenActor)
			{
				endQuaternion = GetEndRotation(actorController);

				actorController.rotation = Quaternion.Slerp(actorController.rotation, endQuaternion, Time.deltaTime * updateTurnTowardsSpeed);
				actorController.localRotation = Quaternion.Euler(actorController.localRotation.eulerAngles.With(x: 0, z: 0));

				return;
			}

			if (distanceBetweenActor == 0 && navMeshAgent.remainingDistance > 0) return;
			HandleEndTurn(actorController);
		}

		void HandleEndTurn(Transform actor)
		{
			if (timer == 0)
			{
				if (distanceBetweenActor == 0) actor.position = moveToTarget.position;

				startQuaternion = actor.rotation;
				endQuaternion = GetEndRotation(actor);
			}

			timer += Time.deltaTime;
			float ratio = timer / endTurnTowardsDuration;

			actor.rotation = Quaternion.Slerp(startQuaternion, endQuaternion, ratio);

			if (ratio >= 1) IsReached = true;
		}

		Quaternion GetEndRotation(Transform actor)
		{
			Vector3 direction = actor.transform.position.GetNormalizedDirectionTo(turnToTarget.position);
			Quaternion endRotation = Quaternion.LookRotation(direction);

			return endRotation;
		}
	}
}