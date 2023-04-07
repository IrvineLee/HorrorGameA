using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Personal.System.Handler
{
	public class FadeHandler : MonoBehaviour
	{
		public Animator Animator { get; private set; }

		public event Action OnFadeOutComplete;

		void Awake()
		{
			Animator = GetComponentInChildren<Animator>();
			Animator.enabled = true;
		}

		/// <summary>
		/// Automatically fades out, does inBetweenAction, wait the delay and fades in.
		/// </summary>
		/// <param name="delay"></param>
		/// <param name="inBetweenAction"></param>
		/// <returns></returns>
		public async UniTask FadeOutInDelay(float delay, Action inBetweenAction = null)
		{
			Action action = async () =>
			{
				int value = (int)(delay * 1000);

				inBetweenAction?.Invoke();
				await UniTask.Delay(value);
			};
			await FadeSequence(action, null);
		}

		/// <summary>
		/// Automatically fades out, does inBetweenTaskList and fades in.
		/// </summary>
		/// <param name="inBetweenTaskList"></param>
		/// <returns></returns>
		public async UniTask FadeOutINAllComplete(List<UniTask> inBetweenTaskList)
		{
			await FadeSequence(null, inBetweenTaskList);
		}

		/// <summary>
		/// Automatically fades out, does inBetweenTaskList, change scene and fades in.
		/// </summary>
		/// <param name="levelIndex"></param>
		/// <param name="inBetweenTaskList"></param>
		/// <param name="animatorUpdateMode"></param>
		/// <returns></returns>
		public async UniTask FadeOutLoadScene(int levelIndex, List<UniTask> inBetweenTaskList = null, AnimatorUpdateMode animatorUpdateMode = AnimatorUpdateMode.Normal)
		{
			await FadeSequence(() => SceneManager.LoadScene(levelIndex), inBetweenTaskList);
		}

		async UniTask FadeIn()
		{
			Animator.SetTrigger("FadeIn");
			await UniTask.DelayFrame(1);
		}

		async UniTask FadeOut()
		{
			Animator.SetTrigger("FadeOut");
			await UniTask.DelayFrame(1);
		}

		async UniTask FadeSequence(Action inBetweenAction, List<UniTask> inBetweenTaskList)
		{
			await FadeOut();
			await FadeComplete();

			if (inBetweenTaskList != null)
				await UniTask.WhenAll(inBetweenTaskList);

			inBetweenAction?.Invoke();

			await FadeIn();
			await FadeComplete();
		}

		async UniTask FadeComplete()
		{
			while (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
			{
				await UniTask.Yield();
			}
		}
	}
}

