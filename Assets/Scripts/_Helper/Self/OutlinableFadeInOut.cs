using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using EPOOutline;

namespace Helper
{
	public class OutlinableFadeInOut : MonoBehaviour
	{
		[SerializeField] float duration = 0.5f;
		[SerializeField] float waitForDuration = 0.2f;
		[SerializeField] float minimumAlpha = 0.4f;

		Outlinable outlinable;
		Color fillColor;
		float fillMaxAlpha;

		CoroutineRun fadeCR = new();

		void Awake()
		{
			outlinable = GetComponentInChildren<Outlinable>();
			fillColor = outlinable.OutlineParameters.FillPass.GetColor("_PublicColor");
			fillMaxAlpha = fillColor.a;
		}

		public void StartFade()
		{
			Action<float> callbackMethod = (result) => SetOutlineColor(result);
			SetOutlineColor(0);

			Fade(true, callbackMethod);
		}

		public void StopFade()
		{
			fadeCR.StopCoroutine();
		}

		async void Fade(bool isFadeIn, Action<float> callbackMethod)
		{
			float startValue = isFadeIn ? minimumAlpha : 1f;
			float endValue = isFadeIn ? 1f : minimumAlpha;

			if (!isFadeIn)
			{
				await UniTask.Delay(waitForDuration.SecondsToMilliseconds());
			}
			fadeCR = CoroutineHelper.LerpWithinSeconds(startValue, endValue, duration, callbackMethod, () => Fade(!isFadeIn, callbackMethod));
		}

		void SetOutlineColor(float result)
		{
			outlinable.FrontParameters.Color = outlinable.FrontParameters.Color.With(a: result);
			outlinable.BackParameters.Color = outlinable.BackParameters.Color.With(a: result);
			outlinable.OutlineParameters.Color = outlinable.OutlineParameters.Color.With(a: result);
			outlinable.OutlineParameters.FillPass.SetColor("_PublicColor", fillColor.With(a: result * fillMaxAlpha));
			outlinable.OutlineParameters.DilateShift = result;
		}
	}
}
