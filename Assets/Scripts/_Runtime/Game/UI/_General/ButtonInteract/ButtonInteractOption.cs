using UnityEngine;

using Personal.Manager;

namespace Personal.UI
{
	public class ButtonInteractOption : ButtonInteractBase
	{
		UISelectable uiSelectable;

		public override void InitialSetup()
		{
			base.InitialSetup();

			uiSelectable = GetComponentInChildren<UISelectable>();
			button.onClick.AddListener(Option);
		}

		void Option()
		{
			UISelectable.AppearSelected(uiSelectable);
			UIManager.Instance.OptionUI.OpenWindow();
		}

		void OnDestroy()
		{
			button.onClick.RemoveAllListeners();
		}
	}
}
