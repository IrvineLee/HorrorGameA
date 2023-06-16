using UnityEngine;

namespace Personal.UI
{
	public class ButtonInteractQuit : ButtonInteractBase
	{
		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(Quit);
		}

		void Quit()
		{
			Application.Quit();
		}

		void OnApplicationQuit()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
