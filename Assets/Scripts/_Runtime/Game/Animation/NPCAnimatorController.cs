using System.Collections.Generic;
using UnityEngine;

using Helper;

namespace Personal.Character.Animation
{
	public class NPCAnimatorController : AnimatorController
	{
		[SerializeField] List<RealAnimatorState<XBotAnimationType>> realAnimatorStateList = new();

		Dictionary<ActorAnimationType, XBotAnimationType> realAnimatorStateDictionary = new();

		public override void Initialize()
		{
			foreach (var state in realAnimatorStateList)
			{
				realAnimatorStateDictionary.Add(state.ActorAnimationType, state.RealAnimationType);
			}
		}

		public override void PlayAnimation(ActorAnimationType actorAnimationType)
		{
			if (realAnimatorStateDictionary.TryGetValue(actorAnimationType, out XBotAnimationType xBotAnimationType))
			{
				animator.CrossFade(xBotAnimationType.GetStringValue(), 0);
			}
		}
	}
}