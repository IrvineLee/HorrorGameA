using System;
using UnityEngine;

namespace Helper
{
	public class CanvasGroupFade : MonoBehaviour
	{
		[SerializeField] float fadeDuration = 1f;

		CanvasGroup canvasGroup;
		CoroutineRun cr = new CoroutineRun();

		void Awake()
		{
			canvasGroup = GetComponentInChildren<CanvasGroup>();
		}

		public void BeginFadeIn()
		{
			Fade(0f, 1f);
		}

		public void BeginFadeOut(Action doLast = default)
		{
			Fade(1f, 0f, doLast);
		}

		void Fade(float startAlpha, float endAlpha, Action doLast = default)
		{
			cr?.StopCoroutine();

			Action<float> callbackMethod = (result) => canvasGroup.alpha = result;
			cr = CoroutineHelper.LerpWithinSeconds(startAlpha, endAlpha, fadeDuration, callbackMethod, doLast);
		}
	}
}
