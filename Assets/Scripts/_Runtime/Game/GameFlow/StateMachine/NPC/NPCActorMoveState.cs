using System;
using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.FSM.Character
{
	[Serializable]
	public class NPCActorMoveState : ActorMoveState
	{
		[SerializeField] protected TargetInfo.TargetType targetType = TargetInfo.TargetType.MoveTo;

		protected override Transform GetTarget()
		{
			var NPCStateMachine = (NPCStateMachine)actorStateMachine;
			TargetInfo targetInfo = NPCStateMachine.TargetInfo;

			switch (targetType)
			{
				case TargetInfo.TargetType.SpawnAt: return targetInfo.SpawnTarget;
				case TargetInfo.TargetType.MoveTo: return targetInfo.MoveToTarget;
				case TargetInfo.TargetType.Leave: return targetInfo.LeaveTarget;
				case TargetInfo.TargetType.Player: return targetInfo.Player;
				default: return null;
			}
		}

		protected override Transform GetTurnTowardsTarget()
		{
			return StageManager.Instance.PlayerController.FSM.transform;
		}
	}
}