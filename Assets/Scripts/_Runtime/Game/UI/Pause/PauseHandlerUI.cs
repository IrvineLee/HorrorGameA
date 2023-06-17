using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Personal.Manager;

namespace Personal.UI.Option
{
	public class PauseHandlerUI : MenuUIBase
	{
		[SerializeField] Button resumeButton = null;
		[SerializeField] Button quitToMainMenuButton = null;

		[Tooltip("Handle buttons that open other gameobject.")]
		//[SerializeField] List<ButtonInteractSet> buttonList = new List<ButtonInteractSet>();

		public override void InitialSetup()
		{
			resumeButton.onClick.AddListener(ResumeButton);
			quitToMainMenuButton.onClick.AddListener(QuitToMainMenu);

			//foreach (var button in buttonList)
			//{
			//	button.Go.SetActive(true);
			//}
		}

		void ResumeButton()
		{
			CloseWindow();
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