using UnityEngine;

using Personal.GameState;

namespace Personal.FSM.Character
{
	public class NPCStateMachine : OrderedStateMachine
	{
		public TargetInfo TargetInfo { get; protected set; }

		public void SetTargetInfo(TargetInfo targetInfo)
		{
			TargetInfo = targetInfo;
		}
	}
}