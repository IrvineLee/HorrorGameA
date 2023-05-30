using System;
using UnityEngine;

using Personal.Manager;

namespace Personal.FSM.Character
{
	[Serializable]
	public class NPCActorMoveState : ActorMoveState
	{
		protected override Transform GetLookAtTarget()
		{
			return StageManager.Instance.PlayerController.FSM.transform;
		}
	}
}