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
		public class RealAnimatorState<T> where T : Enum
		{
			[HorizontalGroup("Split")]
			[VerticalGroup("Split/Left")]
			[LabelWidth(40f)]
			[LabelText("Anim")]
			[SerializeField] ActorAnimationType actorAnimationType;

			[VerticalGroup("Split/Right")]
			[HideLabel()]

			[SerializeField] T realAnimationType;

			public ActorAnimationType ActorAnimationType { get => actorAnimationType; }
			public T RealAnimationType { get => realAnimationType; }

			public RealAnimatorState(ActorAnimationType actorAnimationType, T realAnimationType)
			{
				this.actorAnimationType = actorAnimationType;
				this.realAnimationType = realAnimationType;
			}
		}

		protected Animator animator;

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