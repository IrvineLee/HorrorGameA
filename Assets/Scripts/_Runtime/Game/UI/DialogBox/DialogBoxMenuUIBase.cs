using System;

using Cysharp.Threading.Tasks;

namespace Personal.UI.Dialog
{
	public class DialogBoxMenuUIBase : MenuUIBase
	{
		protected DialogBoxHandlerUI dialogBoxHandlerUI;

		public async override UniTask Initialize()
		{
			await base.Initialize();

			dialogBoxHandlerUI = GetComponentInParent<DialogBoxHandlerUI>();
		}
	}
}
