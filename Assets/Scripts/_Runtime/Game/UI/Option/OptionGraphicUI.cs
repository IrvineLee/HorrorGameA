using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Graphic;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;
using Helper;

namespace Personal.UI.Option
{
	public class OptionGraphicUI : OptionMenuUI
	{
		[Space]
		[SerializeField] TMP_Dropdown screenResolutionDropdown = null;
		[SerializeField] TMP_Dropdown screenModeDropdown = null;
		[SerializeField] TMP_Dropdown qualityDropdown = null;
		[SerializeField] TMP_Dropdown shadowDropdown = null;
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
		List<string> qualityNameList;

		UniversalAdditionalCameraData universalCameraData;

		Resolution currentResolution;
		FullScreenMode currentFullScreenMode;
		int currentQualityIndex;
		int currentShadowIndex;
		int currentAntiAliasIndex;

		Resolution defaultResolution;

		Volume volume;
		Vignette vignette;
		DepthOfField depthOfField;
		MotionBlur motionBlur;
		Bloom bloom;

		protected override void OnPostEnable()
		{
			lastSelectedGO = screenResolutionDropdown.gameObject;
		}

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			universalCameraData = StageManager.Instance.MainCamera.GetComponent<UniversalAdditionalCameraData>();
			defaultResolution = Screen.currentResolution;
			qualityNameList = QualitySettings.names.ToList();

			InitializeVolumeProfile();
			HandleLoadDataToUI();
			RegisterEventsForUI();
			RegisterChangesMadeEvents();
		}

		/// <summary>
		/// OK. Save the value.
		/// </summary>
		public override void Save_Inspector()
		{
			base.Save_Inspector();

			graphicData.SetGraphic(currentResolution, currentFullScreenMode, currentQualityIndex, currentShadowIndex, currentAntiAliasIndex);
			graphicData.SetBoolValue(isVsync.isOn, isVignette.isOn, isDepthOfField.isOn, isMotionBlur.isOn, isBloom.isOn, isAmbientOcclusion.isOn);
		}

		/// <summary>
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			screenResolutionDropdown.onValueChanged.AddListener((index) =>
			{
				currentResolution = resolutionList[index];
				Screen.SetResolution(currentResolution.width, currentResolution.height, currentFullScreenMode);
			});
			screenModeDropdown.onValueChanged.AddListener((index) =>
			{
				currentFullScreenMode = fullscreenModes[index];
				Screen.fullScreenMode = currentFullScreenMode;

				Cursor.lockState = CursorLockMode.Confined;
			});

			qualityDropdown.onValueChanged.AddListener(HandleQuality);
			shadowDropdown.onValueChanged.AddListener(HandleShadow);
			antiAliasingDropdown.onValueChanged.AddListener(HandleAntiAlias);

			isVsync.onValueChanged.AddListener((flag) => QualitySettings.vSyncCount = flag ? 1 : 0);
			isVignette.onValueChanged.AddListener((flag) => vignette.active = flag);
			isDepthOfField.onValueChanged.AddListener((flag) => depthOfField.active = flag);
			isMotionBlur.onValueChanged.AddListener((flag) => motionBlur.active = flag);
			isBloom.onValueChanged.AddListener((flag) => bloom.active = flag);
			isAmbientOcclusion.onValueChanged.AddListener((flag) => HandleAmbientOcclusion(flag));
		}

		/// <summary>
		/// Reset data back to default.
		/// </summary>
		public override void Default_Inspector()
		{
			// Reset data.
			GameStateBehaviour.Instance.SaveProfile.OptionSavedData.ResetGraphicData();

			graphicData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GraphicData;
			graphicData.ScreenResolution = defaultResolution;

			base.Default_Inspector();
		}

		/// <summary>
		/// Reset the data value back to the UI.
		/// </summary>
		protected override void ResetDataToUI()
		{
			graphicData.HandleFirstTimeUser();

			ResetDropdownUI();
			ResetToggleUI();
		}

		/// <summary>
		/// Reset the data value back to the real target.
		/// </summary>
		protected override void ResetDataToTarget()
		{
			Resolution resolution = graphicData.ScreenResolution;
			Screen.SetResolution(resolution.width, resolution.height, graphicData.ScreenMode);

			Screen.fullScreenMode = graphicData.ScreenMode;
			Cursor.lockState = CursorLockMode.Confined;

			HandleShadow(graphicData.ShadowResolution);
			HandleQuality(graphicData.Quality);
			HandleAntiAlias(graphicData.AntiAliasing);
			HandleAmbientOcclusion(graphicData.IsAmbientOcclusion);

			if (!volume) return;

			vignette.active = graphicData.IsVignette;
			depthOfField.active = graphicData.IsDepthOfField;
			motionBlur.active = graphicData.IsMotionBlur;
			bloom.active = graphicData.IsBloom;
		}

		protected override void RegisterChangesMadeEvents()
		{
			unityEventIntList.Add(screenResolutionDropdown.onValueChanged);
			unityEventIntList.Add(screenModeDropdown.onValueChanged);
			unityEventIntList.Add(qualityDropdown.onValueChanged);
			unityEventIntList.Add(shadowDropdown.onValueChanged);
			unityEventIntList.Add(antiAliasingDropdown.onValueChanged);

			unityEventBoolList.Add(isVsync.onValueChanged);
			unityEventBoolList.Add(isVignette.onValueChanged);
			unityEventBoolList.Add(isDepthOfField.onValueChanged);
			unityEventBoolList.Add(isMotionBlur.onValueChanged);
			unityEventBoolList.Add(isBloom.onValueChanged);
			unityEventBoolList.Add(isAmbientOcclusion.onValueChanged);

			base.RegisterChangesMadeEvents();
		}

		/// <summary>
		/// Initialize the post processing from URP.
		/// </summary>
		void InitializeVolumeProfile()
		{
			volume = FindObjectOfType<Volume>();
			if (!volume) return;

			volumeProfile = volume.sharedProfile;
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

			base.HandleLoadDataToUI();
		}

		/// <summary>
		/// Fill up all the resolutions to dropdown.
		/// </summary>
		void InitializeScreenResolution()
		{
			var allResolutions = Screen.resolutions;
			double refreshRate = Screen.currentResolution.refreshRateRatio.value;

			foreach (var resolution in allResolutions)
			{
				double value = resolution.refreshRateRatio.value;
				if (value != refreshRate) continue;

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
				resolution01.refreshRateRatio.value == resolution02.refreshRateRatio.value)
			{
				return true;
			}
			return false;
		}

		void HandleQuality(int index)
		{
			for (int i = 0; i < qualityNameList.Count; i++)
			{
				string quality = qualityNameList[i];
				if (quality.Equals(qualityDropdown.options[index].text))
				{
					QualitySettings.SetQualityLevel(i, true);
					currentQualityIndex = i;
					break;
				}
			}
		}

		void HandleShadow(int index)
		{
			currentShadowIndex = index;

			if (index == 0)
			{
				URPReflection.MainLightCastShadows = false;
				return;
			}

			// Since index 0 is off, minus 1 here to match with the enum.
			URPReflection.MainLightCastShadows = true;
			URPReflection.MainLightShadowResolution = GetShadowResolution(index - 1);
		}

		void HandleAntiAlias(int index)
		{
			// 1 means disabled in URP.
			UniversalRenderPipeline.asset.msaaSampleCount = 1;

			if (index == 3) UniversalRenderPipeline.asset.msaaSampleCount = 2;
			else if (index == 4) UniversalRenderPipeline.asset.msaaSampleCount = 4;
			else if (index == 5) UniversalRenderPipeline.asset.msaaSampleCount = 8;

			universalCameraData.antialiasing = AntialiasingMode.None;

			if (index == 1) universalCameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
			else if (index == 2) universalCameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;

			antiAliasingDropdown.value = index;
			currentAntiAliasIndex = index;
		}

		void HandleAmbientOcclusion(bool flag)
		{
			universalCameraData.SetRenderer(0);

			if (flag)
				universalCameraData.SetRenderer(1);
		}

		void ResetDropdownUI()
		{
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

			for (int i = 0; i < qualityNameList.Count; i++)
			{
				string qualityName = qualityNameList[i];

				Func<bool> func = () => (qualityName.Equals(qualityDropdown.options[graphicData.Quality].text));
				if (ResetDropdown(func, qualityDropdown, ref currentQualityIndex, i)) break;
			}

			int shadowCount = Enum.GetValues(typeof(ShadowResolution)).Length;
			for (int i = 0; i < shadowCount; i++)
			{
				Func<bool> func = () => (i == graphicData.ShadowResolution);
				if (ResetDropdown(func, shadowDropdown, ref currentShadowIndex, i)) break;
			}

			for (int i = 0; i < antiAliasingDropdown.options.Count; i++)
			{
				Func<bool> func = () => (i == graphicData.AntiAliasing);
				if (ResetDropdown(func, antiAliasingDropdown, ref currentAntiAliasIndex, i)) break;
			}
		}

		void ResetToggleUI()
		{
			isVsync.isOn = graphicData.IsVsync;
			isVignette.isOn = graphicData.IsVignette;
			isDepthOfField.isOn = graphicData.IsDepthOfField;
			isMotionBlur.isOn = graphicData.IsMotionBlur;
			isBloom.isOn = graphicData.IsBloom;
			isAmbientOcclusion.isOn = graphicData.IsAmbientOcclusion;
		}

		bool ResetDropdown(Func<bool> condition, TMP_Dropdown tmp_Dropdown, ref int toChangeIndex, int replaceWithIndex)
		{
			if (!condition.Invoke()) return false;

			tmp_Dropdown.value = replaceWithIndex;
			toChangeIndex = replaceWithIndex;

			return true;
		}

		ShadowResolution GetShadowResolution(int index)
		{
			switch (index)
			{
				case 0: return ShadowResolution._256;
				case 1: return ShadowResolution._512;
				case 2: return ShadowResolution._1024;
				case 3: return ShadowResolution._2048;
				case 4: return ShadowResolution._4096;
				default: return ShadowResolution._4096;
			}
		}

		void OnApplicationQuit()
		{
			screenResolutionDropdown.onValueChanged.RemoveAllListeners();
			screenModeDropdown.onValueChanged.RemoveAllListeners();

			isVignette.onValueChanged.RemoveAllListeners();
			isDepthOfField.onValueChanged.RemoveAllListeners();
			isMotionBlur.onValueChanged.RemoveAllListeners();
			isBloom.onValueChanged.RemoveAllListeners();
		}
	}
}