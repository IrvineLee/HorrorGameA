using UnityEngine;
using UnityEngine.EventSystems;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.UI.Option
{
	public class OptionMenuUI : MenuUIBase
	{
		[SerializeField] OptionHandlerUI.MenuTab menuTab = OptionHandlerUI.MenuTab.Graphic;

		public OptionHandlerUI.MenuTab MenuTab { get => menuTab; }

		protected override void OnUpdate()
		{
			if (InputManager.Instance.UIInputController.Move.y == 0) return;

			if (EventSystem.current.currentSelectedGameObject) return;
			if (!lastSelectedGO) return;

			EventSystem.current.SetSelectedGameObject(lastSelectedGO);
		}

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
		public virtual void Save_Inspector() { }

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
