using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;

using Personal.GameState;
using Personal.Manager;
using Personal.Constant;
using Helper;
using Cysharp.Threading.Tasks;

namespace Personal.UI.Option
{
	public class OptionControlUI : OptionMenuUI
	{
		//[SerializeField] Slider masterSlider = null;

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			base.InitialSetup();

			PlayerPrefs.DeleteAll();
			ResetDataToTarget();
			GetComponentsInChildren<UISelectionBase>()?.ToList().ForEach(result => result.Initialize());
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			base.Save_Inspector();

			//string rebinds = InputManager.Instance.PlayerActionInput.asset.SaveBindingOverridesAsJson();
			//PlayerPrefs.SetString(ConstantFixed.CONTROL_MAPPING_PREF_NAME, rebinds);
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