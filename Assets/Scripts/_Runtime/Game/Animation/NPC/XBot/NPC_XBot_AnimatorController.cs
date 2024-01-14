using UnityEngine;

using Helper;

namespace Personal.Character.Animation
{
	public enum XBotAnimationType
	{
		[StringValue("Idle_01")] Idle_01 = 0,
		[StringValue("Walk_01")] Walk_01 = 1000,
	}

	public class NPC_XBot_AnimatorController : ActorAnimatorController<XBotAnimationType>
	{
	}
}