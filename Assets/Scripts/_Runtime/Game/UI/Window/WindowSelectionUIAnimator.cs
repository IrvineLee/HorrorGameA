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

		/// <summary>
		/// Run the animation.
		/// </summary>
		/// <param name="isFlag"></param>
		public void Run(bool isFlag)
		{
			animator.gameObject.SetActive(true);

			animator.StopPlayback();
			windowAnimatorCR.StopCoroutine();

			animator.SetBool(animIsEnable, isFlag);
			windowAnimatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, default);
		}

		/// <summary>
		/// Typically called when needing to disable/reset the animation when the parent is closed.
		/// </summary>
		public void StopAndResetAnimation(bool isDeactivateAnimatorGO = true)
		{
			if (isDeactivateAnimatorGO)
			{
				CoroutineHelper.WaitEndOfFrame(() =>
				{
					// Check whether the reference is still there in case you changed scene.
					if (animator) animator.gameObject.SetActive(false);
				});
			}
			animator.WriteDefaultValues();
		}
	}
}
