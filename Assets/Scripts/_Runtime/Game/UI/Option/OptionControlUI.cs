using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;

using Personal.Manager;
using Personal.Constant;

namespace Personal.UI.Option
{
	public class OptionControlUI : OptionMenuUI
	{
		ControlRebind controlRebind;
		List<UISelectionBase> uiSelectionBaseList = new();

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
			Debug.Log(" Default inspector!!!");
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected override void ResetDataToTarget()
		{
			string rebinds = PlayerPrefs.GetString(ConstantFixed.CONTROL_MAPPING_PREF_NAME);
			if (string.IsNullOrEmpty(rebinds)) return;

			InputManager.Instance.PlayerActionInput.asset.LoadBindingOverridesFromJson(rebinds);
			Debug.Log(" LOADED!!!");
		}
	}
}