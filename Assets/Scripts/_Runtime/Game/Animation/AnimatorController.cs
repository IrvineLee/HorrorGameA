using UnityEngine;

using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.Character.Animation
{
	public class AnimatorController : GameInitialize
	{
		protected Animator animator;

		protected override async UniTask Awake()
		{
			await base.Awake();
			animator = GetComponentInChildren<Animator>();
		}

		public virtual void PlayAnimation(ActorAnimationType actorAnimationType) { }
	}
}