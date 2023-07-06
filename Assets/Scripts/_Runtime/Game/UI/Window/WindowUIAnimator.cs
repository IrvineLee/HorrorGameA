using UnityEngine;

using Helper;

namespace Personal.UI.Window
{
	public class WindowUIAnimator : MonoBehaviour
	{
		[SerializeField] Animator animator;
		[SerializeField] string animationName = "IsEnable";

		public bool IsDone { get => windowAnimatorCR.IsDone; }

		protected CoroutineRun windowAnimatorCR = new();
		protected int animIsEnable;

		void Awake()
		{
			animIsEnable = Animator.StringToHash(animationName);
		}

		public void Run(bool isFlag)
		{
			windowAnimatorCR.StopCoroutine();
			if (isFlag)
			{
				animator.gameObject.SetActive(true);
				animator.SetBool(animIsEnable, true);
				return;
			}

			animator.SetBool(animIsEnable, false);
			windowAnimatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => animator.gameObject.SetActive(false));
		}
	}
}
