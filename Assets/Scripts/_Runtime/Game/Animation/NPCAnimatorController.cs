using System.Collections.Generic;
using UnityEngine;

using Helper;

namespace Personal.Character.Animation
{
	public class NPCAnimatorController : AnimatorController
	{
		[SerializeField] List<AnimatorState<XBotAnimationType>> realAnimatorStateList = new();

		Dictionary<ActorAnimationType, RealAnimatorState<XBotAnimationType>> realAnimatorStateDictionary = new();

		public override void Initialize()
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