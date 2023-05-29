using System;

namespace Personal.UI.Dialog
{
	public class DialogBoxButtonPressOne : DialogBoxButtonPress
	{
		public override void AddListenerToButtonOnce(Action action, string buttonText)
		{
			SetAction(buttonTextInfoList[0], action, buttonText);
		}
	}
}
