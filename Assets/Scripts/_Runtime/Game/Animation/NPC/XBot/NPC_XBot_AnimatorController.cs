using System.Collections.Generic;
using UnityEngine;

using Helper;

namespace Personal.Character.Animation
{
	public class NPC_XBot_AnimatorController : AnimatorController
	{
		public enum XBotAnimationType
		{
			None = 0,

			[StringValue("Idle_01")]
			Idle_01 = 1,

			[StringValue("Walk_01")]
			Walk_01 = 1000,
		}

		[SerializeField] List<AnimatorState<XBotAnimationType>> realAnimatorStateList = new();

		Dictionary<ActorAnimationType, RealAnimatorState<XBotAnimationType>> realAnimatorStateDictionary = new();

		protected override void Initialize()
		{
			foreach (var state in realAnimatorStateList)
			{
				realAnimatorStateDictionary.Add(state.ActorAnimationType, state.RealAnimatorState);
			}
		}

		public override void PlayAnimation(ActorAnimationType actorAnimationType)
		{
			if (realAnimatorStateDictionary.TryGetValue(actorAnimationType, out RealAnimatorState<XBotAnimationType> realState))
			{
				animator.CrossFade(realState.RealAnimationType.GetStringValue(), realState.NormalizedTime);
			}
		}
	}
}