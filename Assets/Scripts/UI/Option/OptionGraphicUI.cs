using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
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
		VolumeProfile volumeProfile;

		Resolution[] resolutions;

		Resolution currentResolution;
		FullScreenMode currentFullScreenMode;

		//UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
		UnityEngine.Rendering.Universal.Vignette vignette;
		UnityEngine.Rendering.Universal.DepthOfField depthOfField;
		UnityEngine.Rendering.Universal.MotionBlur motionBlur;
		UnityEngine.Rendering.Universal.Bloom bloom;

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();

			//InitializeVolumeProfile();
			//HandleLoadDataToUI();
			//RegisterEventsForUI();
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
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			screenResolutionDropdown.onValueChanged.AddListener(ScreenResolutionUpdateRealtime);
		}

		void ScreenResolutionUpdateRealtime(int index)
		{
			Debug.Log("ASDS");
			currentResolution = resolutions[index];
			Screen.SetResolution(currentResolution.width, currentResolution.height, currentFullScreenMode);
		}

		protected override void ResetDataToUI()
		{
			//screenResolutionDropdown
			//displayTypeDropdown

			brightnessSlider.value = graphicData.Brightness.ConvertRatio0To100();
			//antiAliasingDropdown

			isVsync.isOn = graphicData.IsVsync;
			isVignette.isOn = graphicData.IsVignette;
			isDepthOfField.isOn = graphicData.IsDepthOfField;
			isMotionBlur.isOn = graphicData.IsMotionBlur;
			isBloom.isOn = graphicData.IsBloom;
			isAmbientOcclusion.isOn = graphicData.IsAmbientOcclusion;
		}

		protected override void ResetDataToTarget()
		{


			vignette.active = graphicData.IsVsync;
			depthOfField.active = graphicData.IsDepthOfField;
			motionBlur.active = graphicData.IsMotionBlur;
			bloom.active = graphicData.IsBloom;
			//isAmbientOcclusion.active = graphicData.IsAmbientOcclusion;

		}

		void InitializeVolumeProfile()
		{
			volumeProfile = FindObjectOfType<Volume>().profile;
			//volumeProfile.TryGet(out colorAdjustments);
			volumeProfile.TryGet(out vignette);
			volumeProfile.TryGet(out depthOfField);
			volumeProfile.TryGet(out motionBlur);
			volumeProfile.TryGet(out bloom);
		}

		/// <summary>
		/// Display the correct UI from save data.
		/// </summary>
		protected override void HandleLoadDataToUI()
		{
			graphicData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GraphicData;

			HandleScreenResolution(graphicData);
			currentFullScreenMode = graphicData.ScreenMode;

			brightnessSlider.value = graphicData.Brightness.ConvertRatio0To100();
			displayTypeDropdown.value = (int)graphicData.ScreenMode;

			isVsync.isOn = graphicData.IsVsync;
			isVignette.isOn = graphicData.IsVignette;
			isDepthOfField.isOn = graphicData.IsDepthOfField;
			isMotionBlur.isOn = graphicData.IsMotionBlur;
			isBloom.isOn = graphicData.IsBloom;
			isAmbientOcclusion.isOn = graphicData.IsAmbientOcclusion;
		}

		void HandleScreenResolution(GraphicData graphicData)
		{
			int resolutionIndex = -1;
			resolutions = Screen.resolutions;

			foreach (var resolution in resolutions)
			{
				screenResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));

				if (resolution.width == graphicData.ScreenResolution.width &&
					resolution.height == graphicData.ScreenResolution.height &&
					resolution.refreshRate == graphicData.ScreenResolution.refreshRate)
					resolutionIndex = screenResolutionDropdown.options.Count - 1;
			}

			screenResolutionDropdown.value = resolutionIndex;
		}
	}
}