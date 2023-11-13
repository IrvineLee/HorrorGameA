using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Helper
{
	public static class AnimatorExtensions
	{
		// Wait till the animator's animations is over.
		public static async UniTask WaitTillCurrentAnimationOver(this Animator original)
		{
			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => original.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
		}
	}
}
