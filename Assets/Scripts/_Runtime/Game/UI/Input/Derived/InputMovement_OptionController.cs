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

		void IUIControlInput.Next(bool isFlag)
		{
			UIManager.Instance.OptionUI.NextTab(isFlag);
		}

		void IUIControlInput.Default()
		{
			UIManager.Instance.OptionUI.IDefaultHandler.ResetToDefault();
		}
	}
}