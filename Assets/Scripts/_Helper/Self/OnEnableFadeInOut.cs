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

		public void StopFadeAndSetFullVisibility(float waitDuration = 0, Action doLast = default)
		{
			cr.StopCoroutine();

			CoroutineHelper.WaitNextFrame(() =>
			{
				if (sr) FadeSetup_SetFullVisibilityWait(sr, waitDuration, doLast);
				if (image) FadeSetup_SetFullVisibilityWait(image, waitDuration, doLast);
				if (tmp) FadeSetup_SetFullVisibilityWait(tmp, waitDuration, doLast);
			});
		}

		void FadeSetup<T>(T t) where T : Component
		{
			FadeInOut(t, fadeInDuration, waitDuration, fadeOutDuration, isLoop);
		}

		void FadeSetup_SetFullVisibilityWait<T>(T t, float waitDuration = 0, Action doLast = default) where T : Component
		{
			FadeInOut(t, 0, waitDuration, fadeOutDuration, false, doLast);
		}

		void FadeInOut<T>(T t, float fadeInDuration, float waitDuration, float fadeOutDuration, bool isLoop, Action doLast = default) where T : Component
		{
			cr = CoroutineHelper.FadeFromTo(t, 0, 1, fadeInDuration, () =>
			{
				cr = CoroutineHelper.WaitFor(waitDuration, () =>
				{
					cr = CoroutineHelper.FadeFromTo(t, 1, 0, fadeOutDuration, () => FadeEnded(t, fadeInDuration, waitDuration, fadeOutDuration, isLoop, doLast));
				});
			});
		}

		void FadeEnded<T>(T t, float fadeInDuration, float waitDuration, float fadeOutDuration, bool isLoop, Action doLast = default) where T : Component
		{
			if (isLoop)
			{
				FadeInOut(t, fadeInDuration, waitDuration, fadeOutDuration, isLoop, doLast);
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
