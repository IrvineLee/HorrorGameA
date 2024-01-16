using UnityEngine;

namespace Personal.FSM.Character
{
	public class PlayerMoveToInfo
	{
		[SerializeField] Transform moveToTarget = null;
		[SerializeField] Transform turnTowardsTarget = null;
		[SerializeField] float turnTowardsSpeed = 1f;

		public Transform MoveToTarget { get => moveToTarget; }
		public Transform TurnTowardsTarget { get => turnTowardsTarget; }
		public float UpdateTurnTowardsSpeed { get => turnTowardsSpeed; }

		public PlayerMoveToInfo(Transform moveToTarget, Transform turnTowardsTarget, float turnTowardsSpeed)
		{
			this.moveToTarget = moveToTarget;
			this.turnTowardsTarget = turnTowardsTarget;
			this.turnTowardsSpeed = turnTowardsSpeed;
		}
	}
}