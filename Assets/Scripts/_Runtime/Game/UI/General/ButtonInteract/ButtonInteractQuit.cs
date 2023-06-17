using UnityEngine;

using Personal.Manager;
using System;

namespace Personal.UI
{
	public class ButtonInteractQuit : ButtonInteractBase
	{
		public enum QuitType
		{
			TitleScene = 0,
			QuitGame = 100,
		}

		[SerializeField] QuitType quitType = QuitType.TitleScene;

		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(Quit);
		}

		void Quit()
		{
			if (quitType == QuitType.TitleScene)
			{
				Action inBetweenAction = () =>
				{
					UIManager.Instance.CloseAllWindowAndUIInterfaceStack();
				};

				GameSceneManager.Instance.ChangeLevel(SceneName.Title, Transition.TransitionType.Fade, inBetweenAction);

				MenuUIBase.ResumeTime();

				CursorManager.Instance.SetToMouseCursor(false);
				InputManager.Instance.DisableAllActionMap();
			}
			else if (quitType == QuitType.QuitGame)
			{
				Application.Quit();
			}


		}

		void OnApplicationQuit()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
