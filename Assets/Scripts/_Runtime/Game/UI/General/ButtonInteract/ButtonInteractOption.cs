using UnityEngine;

using Personal.Manager;

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
			if (uiGamepadMovement)
			{
				uiGamepadMovement.SetIsUpdate(false);
				UIManager.Instance.OptionUI.SetOnDisableAction(() => uiGamepadMovement.SetIsUpdate(true));
			}

			UIManager.Instance.OptionUI.OpenWindow();
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
