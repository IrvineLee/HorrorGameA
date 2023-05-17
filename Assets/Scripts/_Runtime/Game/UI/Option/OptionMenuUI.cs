using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Personal.UI.Option
{
	public class OptionMenuUI : MenuUIBase
	{
		[SerializeField] OptionHandlerUI.MenuTab menuTab = OptionHandlerUI.MenuTab.Graphic;

		public OptionHandlerUI.MenuTab MenuTab { get => menuTab; }

		OptionHandlerUI optionHandlerUI;

		/// <summary>
		/// Initialize the value before displaying the menu to user.
		/// Typically used to have the data pre-loaded so data is already set when opened.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();
			optionHandlerUI = GetComponentInParent<OptionHandlerUI>();
		}

		/// <summary>
		/// Pressing the OK button.
		/// </summary>
		public override void Save_Inspector()
		{
			optionHandlerUI.CloseMenuTab();
		}

		/// <summary>
		/// Closing the menu.
		/// </summary>
		public override void Cancel_Inspector()
		{
			ResetDataToUI();
			ResetDataToTarget();

			optionHandlerUI.CloseMenuTab();
			gameObject.SetActive(false);
		}

		/// <summary>
		/// Pressing the Default button.
		/// </summary>
		public virtual void Default_Inspector()
		{
			ResetDataToUI();
			ResetDataToTarget();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected virtual void HandleLoadDataToUI()
		{
			ResetDataToUI();
			ResetDataToTarget();
		}

		/// <summary>
		/// Reset the data value back to the UI.
		/// </summary>
		protected virtual void ResetDataToUI() { }

		/// <summary>
		/// Reset the data value back to the real target. Ex: Audio, Graphic etc...
		/// </summary>
		protected virtual void ResetDataToTarget() { }
	}
}
