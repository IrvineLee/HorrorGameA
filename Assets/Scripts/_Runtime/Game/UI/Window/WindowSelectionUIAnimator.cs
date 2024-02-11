using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;

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
			animator.StopPlayback();
			windowAnimatorCR.StopCoroutine();

			if (isFlag)
			{
				animator.gameObject.SetActive(true);
				animator.SetBool(animIsEnable, true);

				windowAnimatorCR = CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, default);
				return;
			}

			if (!animator.gameObject.activeSelf) return;
			animator.SetBool(animIsEnable, false);
		}

		/// <summary>
		/// Typically called when needing to disable/reset the animation when the parent is closed.
		/// </summary>
		public void StopAndResetAnimation()
		{
			animator.gameObject.SetActive(false);
			animator.WriteDefaultValues();
		}
	}
}
