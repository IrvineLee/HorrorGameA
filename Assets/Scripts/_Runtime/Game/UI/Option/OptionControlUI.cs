using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Helper;
using Personal.Manager;
using Personal.Constant;
using Personal.InputProcessing;

namespace Personal.UI.Option
{
	public class OptionControlUI : OptionMenuUI
	{
		[Tooltip("If applicable")]
		[SerializeField] List<Tab> bottomTabList = new();


		ControlRebind controlRebind;
		List<UISelectionBase> uiSelectionBaseList = new();

		protected override void OnEnabled()
		{
			base.OnEnabled();
			UIManager.Instance.OptionUI.UpdateBottomTab(bottomTabList);
		}

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			base.InitialSetup();

			controlRebind = GetComponentInChildren<ControlRebind>();
			controlRebind.InitialSetup();

			ResetDataToTarget();

			uiSelectionBaseList = GetComponentsInChildren<UISelectionBase>(true)?.ToList();
			uiSelectionBaseList.ForEach(result => result.Initialize());
		}


		/// <summary>
		/// Inspector : Only call this when there are bottom tab list.
		/// </summary>
		/// <param name="uiSelectables"></param>
		public void RefreshBottomTabForActiveGO()
		{
			foreach (Tab tab in bottomTabList)
			{
				if (!tab.DisplayGameObject.activeSelf) continue;

				uiSelectableList = tab.DisplayGameObject.GetComponentsInChildren<UISelectable>(true).ToList();
				autoScrollRect = tab.DisplayGameObject.GetComponentInChildren<AutoScrollRect>(true);

				break;
			}
			((BasicControllerUI)ControlInputBase.ActiveControlInput)?.SetUIValues(uiSelectableList, autoScrollRect);
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			base.Save_Inspector();

			string rebinds = InputManager.Instance.PlayerActionInput.asset.SaveBindingOverridesAsJson();
			PlayerPrefs.SetString(ConstantFixed.CONTROL_MAPPING_PREF_NAME, rebinds);
		}

		public override void Default_Inspector()
		{
			// Reset data.
			PlayerPrefs.DeleteKey(ConstantFixed.CONTROL_MAPPING_PREF_NAME);

			controlRebind.InputActionMap.RemoveAllBindingOverrides();
			uiSelectionBaseList.ForEach(result => ((UISelectionSubmit_ControlRebind)result).RefreshUI());

			base.Default_Inspector();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected override void ResetDataToTarget()
		{
			string rebinds = PlayerPrefs.GetString(ConstantFixed.CONTROL_MAPPING_PREF_NAME);
			if (string.IsNullOrEmpty(rebinds)) return;

			InputManager.Instance.PlayerActionInput.asset.LoadBindingOverridesFromJson(rebinds);
		}
	}
}