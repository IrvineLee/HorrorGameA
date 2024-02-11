using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

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
			CloseWindow().Forget();
		}

		void OnApplicationQuit()
		{
			resumeButton.onClick.RemoveAllListeners();
		}
	}
}