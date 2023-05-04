using UnityEngine;

using Personal.GameState;

namespace Personal.FSM
{
	public class ActorMoveToTargetState : ActorMoveAndLookAtState
	{
		[SerializeField] TargetInfo.TargetType targetType = TargetInfo.TargetType.MoveTo;

		public ActorMoveToTargetState(StateMachineBase stateMachine) : base(stateMachine) { }

		protected override Vector3 GetDestination()
		{
			Transform target = actorStateMachine.TargetInfo.SpawnAt;

			if (targetType == TargetInfo.TargetType.MoveTo)
			{
				target = actorStateMachine.TargetInfo.MoveTo;
			}
			else if (targetType == TargetInfo.TargetType.LeaveMoveTo)
			{
				target = actorStateMachine.TargetInfo.LeaveMoveTo;
			}

			return target.position;
		}
	}
}