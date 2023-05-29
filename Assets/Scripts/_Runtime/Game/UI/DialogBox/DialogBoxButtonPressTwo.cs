using System;

namespace Personal.UI.Dialog
{
	public class DialogBoxButtonPressTwo : DialogBoxButtonPress
	{
		public override void AddListenerToButtonOnce(Action action01, Action action02, string buttonText01, string buttonText02)
		{
			SetAction(buttonTextInfoList[0], action01, buttonText01);
			SetAction(buttonTextInfoList[1], action02, buttonText02);
		}
	}
}
