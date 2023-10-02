using System;
using UnityEngine;

using EPOOutline;

namespace Helper
{
	public class OutlinableFadeInOut : MonoBehaviour
	{
		[SerializeField] float duration = 0.5f;
		[SerializeField] float waitForDuration = 0.2f;
		[SerializeField] float minimumAlpha = 0.4f;

		public Outlinable Outlinable { get; private set; }

		Color fillColor;
		float fillMaxAlpha;

		CoroutineRun fadeCR = new();

		void Awake()
		{
			Outlinable = GetComponentInChildren<Outlinable>(true);
			fillColor = Outlinable.OutlineParameters.FillPass.GetColor("_PublicColor");
			fillMaxAlpha = fillColor.a;
		}

		public void StartFade(bool isFlag)
		{
			if (!Outlinable) return;

			Outlinable.enabled = isFlag;

			if (isFlag)
			{
				Action<float> callbackMethod = (result) => SetOutlineColor(result);
				SetOutlineColor(0);

				Fade(true, callbackMethod);
				return;
			}
			fadeCR.StopCoroutine();
		}

		void Fade(bool isFadeIn, Action<float> callbackMethod)
		{
			float startValue = isFadeIn ? minimumAlpha : 1f;
			float endValue = isFadeIn ? 1f : minimumAlpha;

			if (!isFadeIn)
			{
				fadeCR = CoroutineHelper.WaitFor(waitForDuration, () =>
				{
					fadeCR = CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethod, () => Fade(!isFadeIn, callbackMethod));
				});
				return;
			}
			fadeCR = CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethod, () => Fade(!isFadeIn, callbackMethod));
		}

		void SetOutlineColor(float result)
		{
			Outlinable.OutlineParameters.Color = Outlinable.OutlineParameters.Color.With(a: result);
			Outlinable.OutlineParameters.FillPass.SetColor("_PublicColor", fillColor.With(a: result * fillMaxAlpha));
		}
	}
}
