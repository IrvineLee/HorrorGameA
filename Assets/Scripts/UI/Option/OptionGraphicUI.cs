using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
		[SerializeField] TMP_Dropdown screenModeDropdown = null;

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
		FullScreenMode[] fullscreenModes;

		float currentBrightness01;
		Resolution currentResolution;
		FullScreenMode currentFullScreenMode;

		ColorAdjustments colorAdjustments;
		Vignette vignette;
		DepthOfField depthOfField;
		MotionBlur motionBlur;
		Bloom bloom;

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override async UniTask Initialize()
		{
			await base.Initialize();

			InitializeVolumeProfile();
			HandleLoadDataToUI();
			RegisterEventsForUI();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			graphicData.Brightness = currentBrightness01;
			graphicData.SetResolutionAndScreenMode(currentResolution, currentFullScreenMode);
			graphicData.SetBoolValue(isVsync.isOn, isVignette.isOn, isDepthOfField.isOn, isMotionBlur.isOn, isBloom.isOn, isAmbientOcclusion.isOn);
			SaveManager.Instance.SaveProfileData();
		}

		/// <summary>
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			brightnessSlider.onValueChanged.AddListener(BrightnessUpdateRealtime);

			screenResolutionDropdown.onValueChanged.AddListener(ScreenResolutionUpdateRealtime);
			screenModeDropdown.onValueChanged.AddListener(ScreenModeUpdateRealtime);

			isVignette.onValueChanged.AddListener((flag) => vignette.active = flag);
			isDepthOfField.onValueChanged.AddListener((flag) => depthOfField.active = flag);
			isMotionBlur.onValueChanged.AddListener((flag) => motionBlur.active = flag);
			isBloom.onValueChanged.AddListener((flag) => bloom.active = flag);
			//isAmbientOcclusion.onValueChanged.AddListener((flag) => ambi.active = flag);
		}

		/// <summary>
		/// Update screen resolution realtime.
		/// </summary>
		/// <param name="index"></param>
		void ScreenResolutionUpdateRealtime(int index)
		{
			currentResolution = resolutions[index];
			Screen.SetResolution(currentResolution.width, currentResolution.height, currentFullScreenMode);

		}

		void ScreenModeUpdateRealtime(int index)
		{
			currentFullScreenMode = fullscreenModes[index];
			Screen.fullScreenMode = currentFullScreenMode;
		}

		void BrightnessUpdateRealtime(float value)
		{
			currentBrightness01 = value.ConvertRatio0To1();
			colorAdjustments.postExposure.value = currentBrightness01;
		}

		void BoolValueUpdateRealtime(bool flag)
		{

		}

		/// <summary>
		/// Reset data back to default.
		/// </summary>
		public override void Default_Inspector()
		{
			// Reset data.
			graphicData = new GraphicData();
			SaveManager.Instance.SaveProfileData();

			base.Default_Inspector();
		}

		/// <summary>
		/// Reset the data value back to the UI.
		/// </summary>
		protected override void ResetDataToUI()
		{
			brightnessSlider.value = graphicData.Brightness.ConvertRatio0To100();
			//antiAliasingDropdown

			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution resolution = resolutions[i];
				if (IsSameResolution(resolution, graphicData.ScreenResolution))
				{
					screenResolutionDropdown.value = i;
					currentResolution = resolution;
					break;
				}
			}

			for (int i = 0; i < fullscreenModes.Length; i++)
			{
				FullScreenMode fullScreenMode = fullscreenModes[i];
				if (fullScreenMode == graphicData.ScreenMode)
				{
					screenModeDropdown.value = i;
					currentFullScreenMode = fullScreenMode;
					break;
				}
			}

			isVsync.graphic.enabled = graphicData.IsVsync;
			isVignette.graphic.enabled = graphicData.IsVignette;
			isDepthOfField.graphic.enabled = graphicData.IsDepthOfField;
			isMotionBlur.graphic.enabled = graphicData.IsMotionBlur;
			isBloom.graphic.enabled = graphicData.IsBloom;
			isAmbientOcclusion.graphic.enabled = graphicData.IsAmbientOcclusion;
		}

		/// <summary>
		/// Reset the data value back to the real target.
		/// </summary>
		protected override void ResetDataToTarget()
		{
			colorAdjustments.postExposure.value = graphicData.Brightness;

			Resolution resolution = graphicData.ScreenResolution;
			Screen.SetResolution(resolution.width, resolution.height, graphicData.ScreenMode);
			Screen.fullScreenMode = graphicData.ScreenMode;

			vignette.active = graphicData.IsVsync;
			depthOfField.active = graphicData.IsDepthOfField;
			motionBlur.active = graphicData.IsMotionBlur;
			bloom.active = graphicData.IsBloom;
			//isAmbientOcclusion.active = graphicData.IsAmbientOcclusion;
		}

		/// <summary>
		/// Initialize the post processing from URP.
		/// </summary>
		void InitializeVolumeProfile()
		{
			volumeProfile = FindObjectOfType<Volume>().sharedProfile;
			volumeProfile.TryGet(out colorAdjustments);
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

			HandleScreenResolution();
			HandleFullScreenMode();

			brightnessSlider.value = graphicData.Brightness.ConvertRatio0To100();
			screenModeDropdown.value = (int)graphicData.ScreenMode;

			isVsync.isOn = graphicData.IsVsync;
			isVignette.isOn = graphicData.IsVignette;
			isDepthOfField.isOn = graphicData.IsDepthOfField;
			isMotionBlur.isOn = graphicData.IsMotionBlur;
			isBloom.isOn = graphicData.IsBloom;
			isAmbientOcclusion.isOn = graphicData.IsAmbientOcclusion;
		}

		/// <summary>
		/// Fill up all the resolutions to dropdown.
		/// </summary>
		void HandleScreenResolution()
		{
			resolutions = Screen.resolutions;

			foreach (var resolution in resolutions)
			{
				screenResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));

				if (IsSameResolution(resolution, graphicData.ScreenResolution))
				{
					screenResolutionDropdown.value = screenResolutionDropdown.options.Count - 1;
					currentResolution = resolution;
				}
			}
		}

		/// <summary>
		/// Fill up screen mode to dropdown according to OS.
		/// Only handles Windows/MAC as of now.
		/// </summary>
		void HandleFullScreenMode()
		{
			if (UIManager.Instance.IsWindow)
			{
				fullscreenModes = new[] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed };
			}
			else if (UIManager.Instance.IsMAC)
			{
				fullscreenModes = new[] { FullScreenMode.MaximizedWindow, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed };

				// Reset the Window data to Mac data.
				if (graphicData.ScreenMode == FullScreenMode.ExclusiveFullScreen)
					graphicData.ScreenMode = FullScreenMode.MaximizedWindow;
			}

			foreach (var screenMode in fullscreenModes)
			{
				screenModeDropdown.options.Add(new TMP_Dropdown.OptionData(screenMode.ToString()));

				if (screenMode == graphicData.ScreenMode)
				{
					screenModeDropdown.value = screenModeDropdown.options.Count - 1;
					currentFullScreenMode = screenMode;
				}
			}
		}

		/// <summary>
		/// Is both resolution the same.
		/// </summary>
		/// <param name="resolution01"></param>
		/// <param name="resolution02"></param>
		/// <returns></returns>
		bool IsSameResolution(Resolution resolution01, Resolution resolution02)
		{
			if (resolution01.width == resolution02.width &&
				resolution01.height == resolution02.height &&
				resolution01.refreshRate == resolution02.refreshRate)
			{
				return true;
			}
			return false;
		}

		void OnDestroy()
		{
			screenResolutionDropdown.onValueChanged.RemoveAllListeners();
			screenModeDropdown.onValueChanged.RemoveAllListeners();
		}
	}
}