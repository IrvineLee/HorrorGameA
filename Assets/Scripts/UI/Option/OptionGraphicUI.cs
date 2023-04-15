using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

using Personal.GameState;
using Personal.Manager;
using Helper;
using Cysharp.Threading.Tasks;
using Personal.Setting.Graphic;

namespace Personal.UI.Option
{
	public class OptionGraphicUI : OptionMenuUI
	{
		[SerializeField] TMP_Dropdown screenResolutionDropdown = null;
		[SerializeField] TMP_Dropdown displayTypeDropdown = null;

		[SerializeField] Slider brightnessSlider = null;
		[SerializeField] TMP_Dropdown antiAliasingDropdown = null;

		[SerializeField] Toggle isVsync = null;
		[SerializeField] Toggle isVignette = null;
		[SerializeField] Toggle isDepthOfField = null;
		[SerializeField] Toggle isMotionBlur = null;
		[SerializeField] Toggle isBloom = null;
		[SerializeField] Toggle isAmbientOcclusion = null;

		GraphicData graphicData;

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();

			HandleLoadDataToUI();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			graphicData.SetBoolValue(isVsync.isOn, isVignette.isOn, isDepthOfField.isOn, isMotionBlur.isOn, isBloom.isOn, isAmbientOcclusion.isOn);
			SaveManager.Instance.SaveProfileData();
		}

		/// <summary>
		/// Cancel. Close the screen.
		/// </summary>
		public override void Cancel_Inspector()
		{
			//ResetUIValue();
			//ResetRealValue();
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		void HandleLoadDataToUI()
		{
			graphicData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GraphicData;
			Debug.Log(graphicData);

			brightnessSlider.value = graphicData.Brightness.ConvertRatio0To100();
			displayTypeDropdown.value = (int)graphicData.ScreenMode;

			isVsync.isOn = graphicData.IsVsync;
			isVignette.isOn = graphicData.IsVignette;
			isDepthOfField.isOn = graphicData.IsDepthOfField;
			isMotionBlur.isOn = graphicData.IsMotionBlur;
			isBloom.isOn = graphicData.IsBloom;
			isAmbientOcclusion.isOn = graphicData.IsAmbientOcclusion;
		}
	}
}