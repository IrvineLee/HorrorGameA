using System;
using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI.Option
{
	public class PauseHandlerUI : UIHandlerBase
	{
		[SerializeField] Button resumeButton = null;

		protected override void Initialize()
		{
			resumeButton.onClick.AddListener(ResumeButton);
		}

		public override void OpenWindow()
		{
			base.OpenWindow();
			OnPause(true);
		}

		public override void CloseWindow(bool isInstant = false)
		{
			base.CloseWindow(isInstant);
			OnPause(false);
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