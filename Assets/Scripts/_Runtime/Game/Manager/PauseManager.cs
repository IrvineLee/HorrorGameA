using UnityEngine;

using Personal.GameState;
using Personal.UI;

namespace Personal.Manager
{
	public class PauseManager : GameInitializeSingleton<PauseManager>
	{
		public bool IsPaused { get; private set; }

		protected override void OnEarlyMainScene()
		{
			MenuUIBase.OnPauseEvent += Pause;
		}

		/// <summary>
		/// Resume time.
		/// </summary>
		public void ResumeTime()
		{
			Pause(false);
		}

		void Pause(bool isFlag)
		{
			if (IsPaused == isFlag) return;
			if (!GameSceneManager.Instance.IsMainScene()) return;

			IsPaused = isFlag;
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