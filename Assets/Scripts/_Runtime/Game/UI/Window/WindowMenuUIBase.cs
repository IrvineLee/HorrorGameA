using System;

namespace Personal.UI.Window
{
	public class WindowMenuUIBase : MenuUI
	{
		protected WindowHandlerUI windowHandlerUI;

		public override void InitialSetup()
		{
			windowHandlerUI = GetComponentInParent<WindowHandlerUI>();
		}
	}
}
