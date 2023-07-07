using UnityEngine;

using Helper;

namespace Personal.UI.Window
{
	public class WindowSelectionUIAnimator : MonoBehaviour
	{
		[SerializeField] Animator animator;
		[SerializeField] string animationName = "IsEnable";

		public bool IsDone { get => windowAnimatorCR.IsDone; }

		protected CoroutineRun windowAnimatorCR = new();
		protected int animIsEnable;

		public void InitialSetup()
		{
			animIsEnable = Animator.StringToHash(animationName);
		}

		public void Run(bool isFlag)
		{
			if (isFlag)
			{
				// Make sure to stop the coroutine, as stopping it when it started on the same frame does not work.
				CoroutineHelper.WaitNextFrame(() => windowAnimatorCR.StopCoroutine());

				animator.gameObject.SetActive(true);
				animator.SetBool(animIsEnable, true);

				return;
			}

			animator.SetBool(animIsEnable, false);

			windowAnimatorCR.StopCoroutine();
			windowAnimatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => animator.gameObject.SetActive(false), true);
		}
	}
}
