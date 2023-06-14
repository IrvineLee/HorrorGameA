using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Personal.Manager;

namespace Personal.UI.Option
{
	public class PauseHandlerUI : MenuUIBase, IWindowHandler
	{
		[SerializeField] Button resumeButton = null;
		[SerializeField] Button quitToMainMenuButton = null;

		[Tooltip("Handle buttons that open other gameobject.")]
		[SerializeField] List<ButtonOpenSet> buttonList = new List<ButtonOpenSet>();

		public override void InitialSetup()
		{
			IWindowHandler = this;

			resumeButton.onClick.AddListener(ResumeButton);
			quitToMainMenuButton.onClick.AddListener(QuitToMainMenu);

			foreach (var button in buttonList)
			{
				button.Go.SetActive(true);
			}
		}

		void IWindowHandler.OpenWindow()
		{
			UIManager.Instance.WindowStack.Push(this);
		}

		void IWindowHandler.CloseWindow()
		{
			UIManager.Instance.WindowStack.Pop();
		}

		void ResumeButton()
		{
			IWindowHandler.CloseWindow();
		}

		void QuitToMainMenu()
		{
			GameSceneManager.Instance.ChangeLevel(SceneName.Title, Transition.TransitionType.Fade);
		}

		void OnApplicationQuit()
		{
			resumeButton.onClick.RemoveAllListeners();
			quitToMainMenuButton.onClick.RemoveAllListeners();
		}
	}
}