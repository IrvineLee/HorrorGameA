using UnityEngine;

namespace Personal.FSM.Character
{
	public class PlayerMoveToState : ActorMoveState
	{
		public void SetTarget(PlayerMoveToInfo playerMoveToInfo)
		{
			moveToTarget = playerMoveToInfo.MoveToTarget;
			turnTowardsTarget = playerMoveToInfo.TurnTowardsTarget;
			updateTurnTowardsSpeed = playerMoveToInfo.UpdateTurnTowardsSpeed;
		}

		Transform turnTowardsTarget;

		protected override Transform GetTarget()
		{
			return moveToTarget;
		}

		protected override Transform GetTurnTowardsTarget()
		{
			return turnTowardsTarget;
		}
	}
}