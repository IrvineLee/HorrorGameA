using UnityEngine;

using Personal.GameState;

namespace Personal.FSM
{
	public class ActorMoveToTargetState : ActorMoveAndLookAtState
	{
		[SerializeField] TargetInfo.TargetType targetType = TargetInfo.TargetType.MoveTo;

		protected override Vector3 GetDestination()
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
	}
}