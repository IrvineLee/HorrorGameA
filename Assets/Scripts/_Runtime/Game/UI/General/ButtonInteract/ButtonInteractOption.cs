using UnityEngine;

using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI
{
	public class ButtonInteractOption : ButtonInteractBase
	{
		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(Option);
		}

		void Option()
		{
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
			UIManager.Instance.OptionUI.OpenWindow();
		}

		void OnApplicationQuit()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
