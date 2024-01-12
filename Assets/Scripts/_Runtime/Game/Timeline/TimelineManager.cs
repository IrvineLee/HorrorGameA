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
		/// This is called from the timeline signal in the inspector.
		/// </summary>
		/// <param name="target"></param>
		public void LookAtTarget(Transform target)
		{
			var lookAtInfo = new LookAtInfo(target, true, false);
			StageManager.Instance.PlayerController.LookAt(lookAtInfo).Forget();
		}
	}
}