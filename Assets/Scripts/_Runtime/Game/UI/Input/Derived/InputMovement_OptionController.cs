using UnityEngine;

using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI.Option
{
	public class InputMovement_OptionController : BasicControllerUI, IUIControlInput
	{
		void IUIControlInput.Submit()
		{
			UISelectionBase currentSelection = uiSelectableList[CurrentActiveIndex].UISelectionBase;
			currentSelection?.Submit();
		}

		void IUIControlInput.NextShoulder(bool isFlag)
		{
			UIManager.Instance.OptionUI.NextTopTab(isFlag);
		}

		void IUIControlInput.NextTrigger(bool isFlag)
		{
			UIManager.Instance.OptionUI.NextBottomTab(isFlag);
		}

		void IUIControlInput.Default()
		{
			UIManager.Instance.OptionUI.IDefaultHandler.ResetToDefault();
		}
	}
}