using System;
using UnityEngine;

using Personal.Manager;

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

		GameObject inputBlockerGO;

		public override void InitialSetup()
		{
			base.InitialSetup();

			inputBlockerGO = UIManager.Instance.ToolsUI.InputBlocker.gameObject;
			button.onClick.AddListener(Quit);
		}

		void Quit()
		{
			if (quitType == QuitType.TitleScene)
			{
				ReturnToTitleScene();
			}
			else if (quitType == QuitType.QuitGame)
			{
				Application.Quit();
			}
		}

		void ReturnToTitleScene()
		{
			Action inBetweenAction = () =>
			{
				PauseManager.Instance.ResumeTime();

				StageManager.Instance.ResetStage();
				UIManager.Instance.CloseAllWindowAndUIInterfaceStack();
				inputBlockerGO.SetActive(false);
			};

			GameSceneManager.Instance.ChangeLevel(SceneName.Title, inBetweenAction: inBetweenAction, isIgnoreTimescale: true);

			InputManager.Instance.DisableAllActionMap();
			inputBlockerGO.SetActive(true);
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
