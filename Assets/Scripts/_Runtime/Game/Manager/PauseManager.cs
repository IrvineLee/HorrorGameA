using UnityEngine;

using Personal.GameState;
using Personal.UI;

namespace Personal.Manager
{
	public class PauseManager : GameInitializeSingleton<PauseManager>
	{
		protected override void OnEarlyMainScene()
		{
			MenuUIBase.OnPauseEvent += Pause;
		}

		void Pause(bool isFlag)
		{
			Time.timeScale = isFlag ? 0 : 1;

			CursorManager.Instance.SetToMouseCursor(isFlag);
			StageManager.Instance.PlayerController.FPSController.enabled = !isFlag;
		}

		void OnApplicationQuit()
		{
			MenuUIBase.OnPauseEvent -= Pause;
		}
	}
}