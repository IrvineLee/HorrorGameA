using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;

namespace Personal.FSM.Character
{
	public class AnimatorState : StateBase
	{
		[SerializeField] Animator animator = null;
		[SerializeField] string animationName = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			animator.gameObject.SetActive(true);
			animator.enabled = true;

			animator.Play(animationName);
			await animator.WaitTillCurrentAnimationOver();
		}
	}
}