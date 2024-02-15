using System;
using UnityEngine;

using Sirenix.OdinInspector;
using Personal.GameState;

namespace Personal.Character.Animation
{
	public abstract class AnimatorController : GameInitialize
	{
		[Serializable]
		public class AnimatorState<T> where T : Enum
		{
			[HorizontalGroup(LabelWidth = 40, MinWidth = 190f)]
			[LabelText("Anim")]
			[SerializeField] ActorAnimationType actorAnimationType;

			[HorizontalGroup(LabelWidth = 40, MinWidth = 190f)]
			[HideLabel()]
			[SerializeField] RealAnimatorState<T> realAnimatorState;

			public ActorAnimationType ActorAnimationType { get => actorAnimationType; }
			public RealAnimatorState<T> RealAnimatorState { get => realAnimatorState; }
		}

		[Serializable]
		public class RealAnimatorState<T> where T : Enum
		{
			[HorizontalGroup()]
			[HideLabel()]
			[SerializeField] T realAnimationType;

			[HorizontalGroup(MinWidth = 100f)]
			[HideLabel()]
			[Range(0, 1)]
			[SerializeField] float normalizedTime;

			public T RealAnimationType { get => realAnimationType; }
			public float NormalizedTime { get => normalizedTime; }
		}

		public Animator Animator { get; private set; }

		protected override void EarlyInitialize()
		{
			Animator = GetComponentInChildren<Animator>();
		}

		public virtual void PlayAnimation(ActorAnimationType actorAnimationType) { }
		public virtual void ResetAnimationBlend(float duration = 0) { }
	}
}