using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Personal.GameState;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;
using Helper;

namespace Personal.Manager
{
	public class RumbleManager : GameInitializeSingleton<RumbleManager>
	{
		Gamepad gamepad;
		CoroutineRun rumbleCR = new CoroutineRun();

		public void Vibrate(float lowfrequency, float highFrequency, float duration)
		{
			if (InputManager.Instance.InputDeviceType != InputDeviceType.Gamepad) return;

			gamepad = Gamepad.current;
			gamepad.SetMotorSpeeds(lowfrequency, highFrequency);

			rumbleCR?.StopCoroutine();
			rumbleCR = CoroutineHelper.WaitFor(duration, () => gamepad.SetMotorSpeeds(0, 0));
		}
	}
}

