using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Helper
{
	public class OnEnableFadeInOut : MonoBehaviour
	{
		[SerializeField] float fadeInDuration = 1f;
		[SerializeField] float waitDuration = 0f;
		[SerializeField] float fadeOutDuration = 1f;
		[SerializeField] bool isLoop = false;

		SpriteRenderer sr;
		Image image;
		TextMeshProUGUI tmp;

		CoroutineRun cr = new CoroutineRun();

		void Awake()
		{
			sr = GetComponentInChildren<SpriteRenderer>();
			image = GetComponentInChildren<Image>();
			tmp = GetComponentInChildren<TextMeshProUGUI>();
		}

		void OnEnable()
		{
			if (sr) FadeSetup(sr);
			if (image) FadeSetup(image);
			if (tmp) FadeSetup(tmp);
		}

		public void StopFadeAndSetFullVisibility(Action doLast = default)
		{
			cr.StopCoroutine();

			if (sr) FadeInOut(sr, 0, fadeOutDuration, false, doLast);
			if (image) FadeInOut(image, 0, fadeOutDuration, false, doLast);
			if (tmp) FadeInOut(tmp, 0, fadeOutDuration, false, doLast);
		}

		void FadeSetup<T>(T t) where T : Component
		{
			FadeInOut(t, fadeInDuration, fadeOutDuration, isLoop);
		}

		void FadeInOut<T>(T t, float fadeInDuration, float fadeOutDuration, bool isLoop, Action doLast = default) where T : Component
		{
			cr = CoroutineHelper.FadeFromTo(t, 0, 1, fadeInDuration, () =>
			{
				CoroutineHelper.WaitFor(waitDuration, () =>
				{
					cr = CoroutineHelper.FadeFromTo(t, 1, 0, fadeOutDuration, () => FadeEnded(t, fadeInDuration, fadeOutDuration, isLoop, doLast));
				});
			});
		}

		void FadeEnded<T>(T t, float fadeInDuration, float fadeOutDuration, bool isLoop, Action doLast = default) where T : Component
		{
			if (isLoop)
			{
				FadeInOut(t, fadeInDuration, fadeOutDuration, isLoop, doLast);
				return;
			}

			gameObject.SetActive(false);
			doLast?.Invoke();
		}

		void OnDisable()
		{
			cr.StopCoroutine();
		}
	}
}
