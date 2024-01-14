using System;
using System.Collections.Generic;
using UnityEngine;

using Helper;

namespace Personal.Character.Animation
{
	public class ActorAnimatorController<T> : AnimatorController where T : Enum
	{
		[Tooltip("This only affects during ActorMoveState.")]
		[SerializeField] List<AnimatorState<T>> realAnimatorStateList = new();

		Dictionary<ActorAnimationType, RealAnimatorState<T>> realAnimatorStateDictionary = new();

		protected override void EarlyInitialize()
		{
			base.EarlyInitialize();

			foreach (var state in realAnimatorStateList)
			{
				realAnimatorStateDictionary.Add(state.ActorAnimationType, state.RealAnimatorState);
			}
		}

		public override void PlayAnimation(ActorAnimationType actorAnimationType)
		{
			if (realAnimatorStateDictionary.TryGetValue(actorAnimationType, out RealAnimatorState<T> realState))
			{
				Animator.CrossFade(realState.RealAnimationType.GetStringValue(), realState.NormalizedTime);
			}
		}
	}
}