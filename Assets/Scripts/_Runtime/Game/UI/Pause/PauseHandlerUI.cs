using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI.Option
{
	public class PauseHandlerUI : UIHandlerBase
	{
		[SerializeField] Button resumeButton = null;
		[SerializeField] List<ButtonInteractBase> buttonInteractList = new();

		protected override void Initialize()
		{
			resumeButton.onClick.AddListener(ResumeButton);

			foreach (var buttonInteract in buttonInteractList)
			{
				buttonInteract.InitialSetup();
			}
		}

		void ResumeButton()
		{
			CloseWindow();
		}

		void OnApplicationQuit()
		{
			resumeButton.onClick.RemoveAllListeners();
		}
	}
}