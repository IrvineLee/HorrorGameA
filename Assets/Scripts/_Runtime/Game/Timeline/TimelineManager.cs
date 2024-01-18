using UnityEngine;

using Personal.GameState;

namespace Personal.Manager
{
	public class TimelineManager : GameInitializeSingleton<TimelineManager>
	{
		public void DisableGO(Transform target)
		{
			target.gameObject.SetActive(false);
		}
	}
}