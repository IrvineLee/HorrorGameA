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
			UISelectable.CurrentAppearSelected();
			UIManager.Instance.OptionUI.OpenWindow();
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
