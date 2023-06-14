using UnityEngine;
using UnityEngine.EventSystems;

using Personal.Manager;
using Cysharp.Threading.Tasks;

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

		public override async UniTask SetDataToRelevantMember()
		{
			await UniTask.NextFrame();

			ResetData();
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
			ResetData();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected virtual void HandleLoadDataToUI()
		{
			ResetData();
		}

		/// <summary>
		/// Reset the data value back to the UI.
		/// </summary>
		protected virtual void ResetDataToUI() { }

		/// <summary>
		/// Reset the data value back to the real target. Ex: Audio, Graphic etc...
		/// </summary>
		protected virtual void ResetDataToTarget() { }

		void ResetData()
		{
			ResetDataToUI();
			ResetDataToTarget();
		}
	}
}
