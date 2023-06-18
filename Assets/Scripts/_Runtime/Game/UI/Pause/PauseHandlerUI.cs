using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI.Option
{
	public class PauseHandlerUI : MenuUIBase
	{
		[SerializeField] Button resumeButton = null;
		[SerializeField] List<ButtonInteractBase> buttonInteractList = new();

		public override void InitialSetup()
		{
			base.InitialSetup();
			resumeButton.onClick.AddListener(ResumeButton);

			foreach (var buttonInteract in buttonInteractList)
			{
				buttonInteract.InitialSetup();
			}
		}

		protected override void OnEnable()
		{
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
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