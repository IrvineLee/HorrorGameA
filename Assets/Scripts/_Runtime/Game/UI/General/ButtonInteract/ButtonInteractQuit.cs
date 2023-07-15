using System;
using UnityEngine;

using Personal.Manager;
using Personal.Constant;
using Helper;

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
				Action inBetweenAction = () =>
				{
					PauseManager.Instance.ResumeTime();

					StageManager.Instance.PlayerController.Inventory.ResetInventoryUI();
					UIManager.Instance.CloseAllWindowAndUIInterfaceStack();
					inputBlockerGO.SetActive(false);
				};

				GameSceneManager.Instance.ChangeLevel(SceneType.Title.GetStringValue(), inBetweenAction: inBetweenAction, isIgnoreTimescale: true);

				InputManager.Instance.DisableAllActionMap();
				inputBlockerGO.SetActive(true);
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
