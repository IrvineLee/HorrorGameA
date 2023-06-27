using System;
using UnityEngine;

using Helper;

namespace Personal.Character.Player
{
	public class LightFlickerOnOff : MonoBehaviour
	{
		[Tooltip("Minimum duration uptime")]
		[SerializeField] float onMinDuration = 0f;
		[Tooltip("Maximum duration uptime")]
		[SerializeField] float onMaxDuration = 1f;
		[Tooltip("Minimum duration downtime")]
		[SerializeField] float offMinDuration = 0f;
		[Tooltip("Maximum duration downtime")]
		[SerializeField] float offMaxDuration = 1f;

		new Light light;
		CoroutineRun flickeringCR = new CoroutineRun();

		void Start()
		{
			light = GetComponentInChildren<Light>();
			Flicker(true);
		}

		public void Flicker(bool isFlag)
		{
			if (!isFlag)
			{
				flickeringCR.StopCoroutine();
				return;
			}

			CoroutineRun waitCR = new CoroutineRun();
			Action action = () =>
			{
				if (!waitCR.IsDone) return;

				float random = UnityEngine.Random.Range(onMinDuration, onMaxDuration);
				light.enabled = true;

				waitCR = CoroutineHelper.WaitFor(random, () =>
				{
					if (flickeringCR.IsDone) return;
					light.enabled = false;

					random = UnityEngine.Random.Range(offMinDuration, offMaxDuration);
					waitCR = CoroutineHelper.WaitFor(random, default);
				});
			};

			flickeringCR = CoroutineHelper.RunActionUntilBreak(0, action, default);
		}

		void OnDestroy()
		{
			flickeringCR?.StopCoroutine();
		}
	}
}