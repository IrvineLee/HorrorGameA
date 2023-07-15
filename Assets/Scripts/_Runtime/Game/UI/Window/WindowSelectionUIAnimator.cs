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
			windowAnimatorCR.StopCoroutine();

			if (isFlag)
			{
				// Make sure to stop the coroutine, as stopping it when it started on the same frame does not work.
				CoroutineHelper.WaitNextFrame(() => windowAnimatorCR.StopCoroutine());

				animator.gameObject.SetActive(true);
				animator.SetBool(animIsEnable, true);

				return;
			}

			if (!animator.gameObject.activeSelf) return;
			animator.SetBool(animIsEnable, false);

			windowAnimatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => animator.gameObject.SetActive(false), true);
		}

		/// <summary>
		/// Typically called when needing to disable/reset the animation when the parent is closed.
		/// </summary>
		public void StopAnimation()
		{
			animator.gameObject.SetActive(false);

			if (!animator.gameObject.activeSelf) return;
			animator.SetBool(animIsEnable, false);
		}
	}
}
