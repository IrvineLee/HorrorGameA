using UnityEngine;

using Personal.Manager;

namespace Personal.UI
{
	public class ButtonInteractNewGame : ButtonInteractBase
	{
		public override void InitialSetup()
		{
			base.InitialSetup();
			button.onClick.AddListener(NewGame);
		}

		void NewGame()
		{
			// Make sure to use new data.
			SaveManager.Instance.LoadSlotData(-1);
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
