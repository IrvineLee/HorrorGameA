using UnityEngine;

using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI.Option
{
	public class InputMovement_OptionController : BasicControllerUI, IControlInput
	{
		void IControlInput.Next(bool isFlag)
		{
			UIManager.Instance.OptionUI.NextTab(isFlag);
		}

		void IControlInput.R3()
		{
			UIManager.Instance.OptionUI.IDefaultHandler.ResetToDefault();
		}
	}
}