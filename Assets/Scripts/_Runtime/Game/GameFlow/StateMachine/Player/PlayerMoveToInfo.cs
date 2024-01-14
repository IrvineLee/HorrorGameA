using UnityEngine;

namespace Personal.FSM.Character
{
	public class PlayerMoveToInfo
	{
		[SerializeField] Transform moveToTarget = null;
		[SerializeField] Transform turnTowardsTarget = null;
		[SerializeField] Transform lookAtTarget = null;
		[SerializeField] float turnTowardsSpeed = 5f;

		public Transform MoveToTarget { get => moveToTarget; }
		public Transform TurnTowardsTarget { get => turnTowardsTarget; }
		public float TurnTowardsSpeed { get => turnTowardsSpeed; }

		public PlayerMoveToInfo(Transform moveToTarget, Transform turnTowardsTarget, float turnTowardsSpeed = 5f)
		{
			this.moveToTarget = moveToTarget;
			this.turnTowardsTarget = turnTowardsTarget;
			this.turnTowardsSpeed = turnTowardsSpeed;
		}
	}
}