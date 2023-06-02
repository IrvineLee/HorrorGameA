using System;

namespace Personal.UI.Window
{
	public class WindowButtonPressOne : WindowButtonPress
	{
		public override void AddListenerToButtonOnce(Action action, string buttonText)
		{
			SetAction(buttonTextInfoList[0], action, buttonText);
		}
	}
}
