using System;

using Cysharp.Threading.Tasks;

namespace Personal.UI.Window
{
	public class WindowMenuUIBase : MenuUIBase
	{
		protected WindowHandlerUI windowHandlerUI;

		public async override UniTask Initialize()
		{
			await base.Initialize();

			windowHandlerUI = GetComponentInParent<WindowHandlerUI>();
		}
	}
}
