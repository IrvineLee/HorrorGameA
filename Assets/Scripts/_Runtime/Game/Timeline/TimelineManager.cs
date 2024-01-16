using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.FSM.Character;

namespace Personal.Manager
{
	public class TimelineManager : GameInitializeSingleton<TimelineManager>
	{
		/// <summary>
		/// Player look at target
		/// </summary>
		/// <param name="target"></param>
		public void LookAtTarget(Transform target)
		{
			var lookAtInfo = new LookAtInfo(target, true, false);
			StageManager.Instance.PlayerController.LookAt(lookAtInfo).Forget();
		}

		/// <summary>
		/// Call this to make the camera's rotation persist after the timeline is finished.
		/// </summary>
		/// <param name="value"></param>
		public void SetCameraTargetPitch(float value)
		{
			StageManager.Instance.PlayerController.FPSController.UpdateTargetPitch(value);
		}

		public void EnableGO(Transform target)
		{
			target.gameObject.SetActive(true);
		}

		public void DisableGO(Transform target)
		{
			target.gameObject.SetActive(false);
		}
	}
}