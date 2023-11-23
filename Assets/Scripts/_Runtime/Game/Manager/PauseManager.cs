using UnityEngine;

using Personal.GameState;
using Personal.UI;

namespace Personal.Manager
{
	public class PauseManager : GameInitializeSingleton<PauseManager>
	{
		public bool IsPaused { get; private set; }

		/// <summary>
		/// Resume time.
		/// </summary>
		public void ResumeTime()
		{
			Pause(false);
		}

		protected override void Initialize()
		{
			MenuUI.OnPauseEvent += Pause;
		}

		void Pause(bool isFlag)
		{
			if (IsPaused == isFlag) return;
			if (!GameSceneManager.Instance.IsMainScene()) return;

			IsPaused = isFlag;
			Time.timeScale = isFlag ? 0 : 1;

			StageManager.Instance.PlayerController.PauseFSM(isFlag);

			// You don't wanna affect the cursor during dialogue.
			if (UIManager.Instance.ActiveInterfaceType == UIInterfaceType.Dialogue) return;

			CursorManager.Instance.TrySetToMouseCursorForMouseControl(isFlag, isFlag);
		}

		void OnApplicationQuit()
		{
			MenuUI.OnPauseEvent -= Pause;
		}
	}
}