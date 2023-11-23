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