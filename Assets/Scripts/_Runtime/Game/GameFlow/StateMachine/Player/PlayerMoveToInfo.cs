using UnityEngine;

namespace Personal.FSM.Character
{
	public class PlayerMoveToInfo
	{
		[SerializeField] Transform moveToTarget = null;
		[SerializeField] Transform lookAtTarget = null;
		[SerializeField] float turnTowardsSpeed = 1f;

		public Transform MoveToTarget { get => moveToTarget; }
		public Transform LookAtTarget { get => lookAtTarget; }
		public float UpdateTurnTowardsSpeed { get => turnTowardsSpeed; }

		public PlayerMoveToInfo(Transform moveToTarget, Transform lookAtTarget, float turnTowardsSpeed)
		{
			this.moveToTarget = moveToTarget;
			this.lookAtTarget = lookAtTarget;
			this.turnTowardsSpeed = turnTowardsSpeed;
		}
	}
}