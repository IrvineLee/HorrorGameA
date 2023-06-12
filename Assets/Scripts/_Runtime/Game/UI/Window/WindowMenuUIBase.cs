using System;

namespace Personal.UI.Window
{
	public class WindowMenuUIBase : MenuUIBase
	{
		protected WindowHandlerUI windowHandlerUI;

		public override void InitialSetup()
		{
			windowHandlerUI = GetComponentInParent<WindowHandlerUI>();
		}
	}
}
