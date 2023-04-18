using System.Collections.Generic;
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

		List<Resolution> resolutionList = new List<Resolution>();
		FullScreenMode[] fullscreenModes = GameManager.IsWindow ?
										   new[] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed } :
										   new[] { FullScreenMode.MaximizedWindow, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed };

		float currentBrightness01;
		Resolution currentResolution;
		FullScreenMode currentFullScreenMode;

		Resolution defaultResolution;

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

			defaultResolution = Screen.currentResolution;

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
			brightnessSlider.onValueChanged.AddListener((value) =>
			{
				currentBrightness01 = value.ConvertRatio0To1();
				colorAdjustments.postExposure.value = currentBrightness01;
			});

			screenResolutionDropdown.onValueChanged.AddListener((index) =>
			{
				currentResolution = resolutionList[index];
				Screen.SetResolution(currentResolution.width, currentResolution.height, currentFullScreenMode);
			});
			screenModeDropdown.onValueChanged.AddListener((index) =>
			{
				currentFullScreenMode = fullscreenModes[index];
				Screen.fullScreenMode = currentFullScreenMode;

				HandleFullScreenMode(currentFullScreenMode);
			});

			isVignette.onValueChanged.AddListener((flag) => vignette.active = flag);
			isDepthOfField.onValueChanged.AddListener((flag) => depthOfField.active = flag);
			isMotionBlur.onValueChanged.AddListener((flag) => motionBlur.active = flag);
			isBloom.onValueChanged.AddListener((flag) => bloom.active = flag);
			//isAmbientOcclusion.onValueChanged.AddListener((flag) => ambi.active = flag);
		}

		/// <summary>
		/// Reset data back to default.
		/// </summary>
		public override void Default_Inspector()
		{
			// Reset data.
			graphicData = new GraphicData();
			SaveManager.Instance.SaveProfileData();

			graphicData.ScreenResolution = defaultResolution;
			base.Default_Inspector();
		}

		/// <summary>
		/// Reset the data value back to the UI.
		/// </summary>
		protected override void ResetDataToUI()
		{
			brightnessSlider.value = graphicData.Brightness.ConvertRatio0To100();
			//antiAliasingDropdown

			// Set current resolution on the first time.
			if (graphicData.ScreenResolution.width == 0 && graphicData.ScreenResolution.height == 0)
			{
				graphicData.ScreenResolution = Screen.currentResolution;
			}

			for (int i = 0; i < resolutionList.Count; i++)
			{
				Resolution resolution = resolutionList[i];
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

			isVsync.isOn = graphicData.IsVsync;
			isVignette.isOn = graphicData.IsVignette;
			isDepthOfField.isOn = graphicData.IsDepthOfField;
			isMotionBlur.isOn = graphicData.IsMotionBlur;
			isBloom.isOn = graphicData.IsBloom;
			isAmbientOcclusion.isOn = graphicData.IsAmbientOcclusion;
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

			HandleFullScreenMode(graphicData.ScreenMode);

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

			InitializeScreenResolution();
			InitializeFullScreenMode();

			ResetDataToUI();
			ResetDataToTarget();
		}

		/// <summary>
		/// Fill up all the resolutions to dropdown.
		/// </summary>
		void InitializeScreenResolution()
		{
			var allResolutions = Screen.resolutions;
			int refreshRate = Screen.currentResolution.refreshRate;

			foreach (var resolution in allResolutions)
			{
				if (resolution.refreshRate != refreshRate) continue;

				screenResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + " x " + resolution.height));
				resolutionList.Add(resolution);

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
		void InitializeFullScreenMode()
		{
			// Reset the Window data to Mac data.
			if (graphicData.ScreenMode == FullScreenMode.ExclusiveFullScreen)
				graphicData.ScreenMode = FullScreenMode.MaximizedWindow;

			foreach (var screenMode in fullscreenModes)
			{
				string s = "Windowed";
				if (screenMode == FullScreenMode.ExclusiveFullScreen || screenMode == FullScreenMode.MaximizedWindow)
					s = "Fullscreen";
				else if (screenMode == FullScreenMode.FullScreenWindow)
					s = "Borderless Fullscreen";

				screenModeDropdown.options.Add(new TMP_Dropdown.OptionData(s));

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

		/// <summary>
		/// Handle whether the cursor is confined.
		/// </summary>
		/// <param name="fullScreenMode"></param>
		void HandleFullScreenMode(FullScreenMode fullScreenMode)
		{
			Cursor.lockState = CursorLockMode.None;

			if (fullScreenMode == FullScreenMode.ExclusiveFullScreen ||
				fullScreenMode == FullScreenMode.MaximizedWindow)
			{
				Cursor.lockState = CursorLockMode.Confined;
			}
		}

		void OnDestroy()
		{
			brightnessSlider.onValueChanged.RemoveAllListeners();

			screenResolutionDropdown.onValueChanged.RemoveAllListeners();
			screenModeDropdown.onValueChanged.RemoveAllListeners();

			isVignette.onValueChanged.RemoveAllListeners();
			isDepthOfField.onValueChanged.RemoveAllListeners();
			isMotionBlur.onValueChanged.RemoveAllListeners();
			isBloom.onValueChanged.RemoveAllListeners();
		}
	}
}