using UnityEngine;
using UnityEngine.UI;

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

		CoroutineRun cr;

		void Awake()
		{
			sr = GetComponentInChildren<SpriteRenderer>();
			image = GetComponentInChildren<Image>();
		}

		void OnEnable()
		{
			if (sr) FadeInOut(sr);
			if (image) FadeInOut(image);
		}

		void FadeInOut<T>(T t) where T : Component
		{
			cr = CoroutineHelper.FadeFromTo(t, 0, 1, fadeInDuration, () =>
			{
				CoroutineHelper.WaitFor(waitDuration, () =>
				{
					cr = CoroutineHelper.FadeFromTo(t, 1, 0, fadeOutDuration, () =>
					{
						if (isLoop) FadeInOut(t);
						else gameObject.SetActive(false);
					});
				});
			});
		}

		void OnDisable()
		{
			cr.StopCoroutine();
		}
	}
}
