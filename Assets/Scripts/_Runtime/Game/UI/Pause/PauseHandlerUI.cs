using UnityEngine;
using UnityEngine.UI;

using Personal.Manager;

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
			StageManager.Instance.PlayerController.PauseFSM(true);
		}

		public override void CloseWindow(bool isInstant = false)
		{
			base.CloseWindow(isInstant);
			StageManager.Instance.PlayerController.PauseFSM(false);
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