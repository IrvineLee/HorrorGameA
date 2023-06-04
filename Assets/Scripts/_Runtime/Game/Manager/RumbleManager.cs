using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Personal.GameState;
using Personal.InputProcessing;
using Helper;

namespace Personal.Manager
{
	public class RumbleManager : GameInitializeSingleton<RumbleManager>
	{
		CoroutineRun rumbleCR = new CoroutineRun();

		public void Vibrate(float lowfrequency, float highFrequency, float duration)
		{
			if (InputManager.Instance.InputDeviceType != InputDeviceType.Gamepad) return;

			Gamepad gamepad = InputManager.Instance.CurrentGamepad;
			gamepad.SetMotorSpeeds(lowfrequency, highFrequency);

			rumbleCR?.StopCoroutine();
			rumbleCR = CoroutineHelper.WaitFor(duration, () => gamepad.SetMotorSpeeds(0, 0), default, true);
		}
	}
}

