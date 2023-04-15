using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Personal.UI.Option
{
	public class OptionMenuUI : MenuUIBase
	{
		[SerializeField] OptionHandlerUI.MenuTab menuTab = OptionHandlerUI.MenuTab.Graphic;

		public OptionHandlerUI.MenuTab MenuTab { get => menuTab; }

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();
		}

		/// <summary>
		/// Pressing the OK button.
		/// </summary>
		public override void Save_Inspector() { }

		/// <summary>
		/// Closing the menu.
		/// </summary>
		public override void Cancel_Inspector() { }
	}
}
