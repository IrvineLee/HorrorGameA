using UnityEngine;

namespace Personal.UI
{
	public class ButtonInteractQuit : ButtonInteractSet
	{
		public override void Initialize()
		{
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
