using UnityEngine;

using Personal.Manager;

namespace Personal.UI.Option
{
	public class OptionControllerUI : ControlInputUI
	{
		protected override void Next(bool isFlag)
		{
			UIManager.Instance.OptionUI.NextTab(isFlag);
		}

		protected override void ButtonNorth()
		{
			UIManager.Instance.OptionUI.IDefaultHandler.ResetToDefault();
		}
	}
}