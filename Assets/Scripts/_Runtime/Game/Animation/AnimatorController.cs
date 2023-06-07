using System;
using UnityEngine;

using Personal.GameState;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace Personal.Character.Animation
{
	public class AnimatorController : GameInitialize
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

		protected Animator animator;

		public Animator Animator { get => animator; }

		protected override async UniTask Awake()
		{
			await base.Awake();
			animator = GetComponentInChildren<Animator>();

			Initialize();
		}

		public virtual void Initialize() { }

		public virtual void PlayAnimation(ActorAnimationType actorAnimationType) { }
	}
}