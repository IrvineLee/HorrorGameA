using UnityEngine;

using Helper;
using Personal.GameState;

namespace Personal.UI
{
	public class GameObjectAnimatorHandler : GameInitialize
	{
		[SerializeField] Animator animator;
		[SerializeField] string animationName = "IsEnable";

		protected CoroutineRun animatorCR = new();
		protected int animIsEnable;

		public virtual void Begin(bool isFlag)
		{
			animatorCR?.StopCoroutine();
			if (animIsEnable == 0) animIsEnable = Animator.StringToHash(animationName);

			if (isFlag) gameObject.SetActive(isFlag);
			animator.SetBool(animIsEnable, isFlag);

			if (!isFlag)
			{
				animatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => animator.gameObject.SetActive(false), true);
			}
		}
	}
}
