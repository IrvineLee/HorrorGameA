using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
		[SerializeField] UISelectionDropdown screenResolutionDropdown = null;
		[SerializeField] UISelectionDropdown screenModeDropdown = null;
		[SerializeField] UISelectionDropdown qualityDropdown = null;
		[SerializeField] UISelectionDropdown shadowDropdown = null;
		[SerializeField] UISelectionDropdown antiAliasingDropdown = null;

		[SerializeField] UISelectionToggle isVsync = null;
		[SerializeField] UISelectionToggle isVignette = null;
		[SerializeField] UISelectionToggle isDepthOfField = null;
		[SerializeField] UISelectionToggle isMotionBlur = null;
		[SerializeField] UISelectionToggle isBloom = null;
		[SerializeField] UISelectionToggle isAmbientOcclusion = null;

		// CAUTION : This value follows the index of the dropdown list. If it gets changed in the future, change here too.
		const int FXAAIndex = 1;
		const int SMAAIndex = 2;
		const int MSAAIndex2 = 3;
		const int MSAAIndex4 = 4;
		const int MSAAIndex8 = 5;

		GraphicData graphicData;
		VolumeProfile volumeProfile;

		List<Resolution> resolutionList = new List<Resolution>();
		FullScreenMode[] fullscreenModes = GameManager.IsWindow ?
										   new[] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed } :
										   new[] { FullScreenMode.MaximizedWindow, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed };
		List<string> qualityNameList;

		UniversalAdditionalCameraData universalCameraData;

		Resolution currentResolution;
		int currentFullScreenMode;
		int currentQualityIndex;
		int currentShadowIndex;
		int currentAntiAliasIndex;

		Resolution defaultResolution;

		Volume volume;
		Vignette vignette;
		DepthOfField depthOfField;
		MotionBlur motionBlur;
		Bloom bloom;

		protected override void OnEnabled()
		{
			base.OnEnabled();
			lastSelectedGO = screenResolutionDropdown.gameObject;
		}

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <returns></returns>
		public override void InitialSetup()
		{
			base.InitialSetup();

			GetComponentsInChildren<UISelectionBase>()?.ToList().ForEach(result => result.Initialize());

			universalCameraData = StageManager.Instance.CameraHandler.UniversalAdditionalCameraData;
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
			graphicData.SetBoolValue(isVsync.IsOn, isVignette.IsOn, isDepthOfField.IsOn, isMotionBlur.IsOn, isBloom.IsOn, isAmbientOcclusion.IsOn);
		}

		/// <summary>
		/// Register events for real-time update.
		/// </summary>
		void RegisterEventsForUI()
		{
			screenResolutionDropdown.OnValueChanged.AddListener((index) =>
			{
				currentResolution = resolutionList[index];
				Screen.SetResolution(currentResolution.width, currentResolution.height, (FullScreenMode)currentFullScreenMode);
			});
			screenModeDropdown.OnValueChanged.AddListener((index) =>
			{
				currentFullScreenMode = index;
				Screen.fullScreenMode = fullscreenModes[index];
				Cursor.lockState = CursorLockMode.Confined;
			});

			qualityDropdown.OnValueChanged.AddListener(HandleQuality);
			shadowDropdown.OnValueChanged.AddListener(HandleShadow);
			antiAliasingDropdown.OnValueChanged.AddListener(HandleAntiAlias);

			isVsync.OnValueChanged.AddListener((flag) => QualitySettings.vSyncCount = flag ? 1 : 0);
			isVignette.OnValueChanged.AddListener((flag) => vignette.active = flag);
			isDepthOfField.OnValueChanged.AddListener((flag) => depthOfField.active = flag);
			isMotionBlur.OnValueChanged.AddListener((flag) => motionBlur.active = flag);
			isBloom.OnValueChanged.AddListener((flag) => bloom.active = flag);
			isAmbientOcclusion.OnValueChanged.AddListener((flag) => HandleAmbientOcclusion(flag));
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
			Screen.SetResolution(resolution.width, resolution.height, (FullScreenMode)graphicData.ScreenMode);

			Screen.fullScreenMode = (FullScreenMode)graphicData.ScreenMode;
			Cursor.lockState = CursorLockMode.Confined;

			HandleShadow(graphicData.ShadowResolution);
			HandleQuality(graphicData.Quality);
			HandleAntiAlias(graphicData.AntiAliasing);
			HandleAmbientOcclusion(graphicData.IsAmbientOcclusion);

			if (!volume) return;

			if (vignette) vignette.active = graphicData.IsVignette;
			if (depthOfField) depthOfField.active = graphicData.IsDepthOfField;
			if (motionBlur) motionBlur.active = graphicData.IsMotionBlur;
			if (bloom) bloom.active = graphicData.IsBloom;
		}

		protected override void RegisterChangesMadeEvents()
		{
			unityEventIntList.Add(screenResolutionDropdown.OnValueChanged);
			unityEventIntList.Add(screenModeDropdown.OnValueChanged);
			unityEventIntList.Add(qualityDropdown.OnValueChanged);
			unityEventIntList.Add(shadowDropdown.OnValueChanged);
			unityEventIntList.Add(antiAliasingDropdown.OnValueChanged);

			unityEventBoolList.Add(isVsync.OnValueChanged);
			unityEventBoolList.Add(isVignette.OnValueChanged);
			unityEventBoolList.Add(isDepthOfField.OnValueChanged);
			unityEventBoolList.Add(isMotionBlur.OnValueChanged);
			unityEventBoolList.Add(isBloom.OnValueChanged);
			unityEventBoolList.Add(isAmbientOcclusion.OnValueChanged);

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

			List<string> resolutionDisplayList = new();
			foreach (var resolution in allResolutions)
			{
				double value = resolution.refreshRateRatio.value;
				if (value != refreshRate) continue;

				resolutionDisplayList.Add(resolution.width + " x " + resolution.height);
				resolutionList.Add(resolution);

				if (IsSameResolution(resolution, graphicData.ScreenResolution))
				{
					currentResolution = resolution;
				}
			}
			screenResolutionDropdown.UpdateListAndInitalize(resolutionDisplayList);
		}

		/// <summary>
		/// Fill up screen mode to dropdown according to OS.
		/// Handles Windows/MAC as of now.
		/// </summary>
		void InitializeFullScreenMode()
		{
			List<string> screenModeDisplayList = new();
			foreach (var screenMode in fullscreenModes)
			{
				string s = "Windowed";
				if (screenMode == FullScreenMode.ExclusiveFullScreen || screenMode == FullScreenMode.MaximizedWindow)
					s = "Fullscreen";
				else if (screenMode == FullScreenMode.FullScreenWindow)
					s = "Borderless Fullscreen";

				screenModeDisplayList.Add(s);

				if (screenMode == (FullScreenMode)graphicData.ScreenMode)
				{
					currentFullScreenMode = (int)screenMode;
				}
			}
			screenModeDropdown.UpdateListAndInitalize(screenModeDisplayList);
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
				if (quality.Equals(qualityDropdown.StringList[index]))
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

			if (index == MSAAIndex2) UniversalRenderPipeline.asset.msaaSampleCount = 2;
			else if (index == MSAAIndex4) UniversalRenderPipeline.asset.msaaSampleCount = 4;
			else if (index == MSAAIndex8) UniversalRenderPipeline.asset.msaaSampleCount = 8;

			universalCameraData.antialiasing = AntialiasingMode.None;

			if (index == FXAAIndex) universalCameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
			else if (index == SMAAIndex) universalCameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;

			antiAliasingDropdown.SetCurrentIndex(index);
			currentAntiAliasIndex = index;

			HandleAmbientOcclusion(isAmbientOcclusion.IsOn);
		}

		/// <summary>
		/// Following the rendering list. If the list changes, update here.
		/// 0. URP_Renderer (default)
		/// 1. URP_Renderer_Forward
		/// 2. URP_AmbientOcclusion
		/// 3. URP_AmbientOcclusion_Forward
		/// </summary>
		/// <param name="isFlag"></param>
		void HandleAmbientOcclusion(bool isFlag)
		{
			bool isMSAAAntiAlias = currentAntiAliasIndex >= MSAAIndex2 && currentAntiAliasIndex <= MSAAIndex8;

			if (isFlag)
			{
				universalCameraData.SetRenderer(isMSAAAntiAlias ? 3 : 2); // URP_AmbientOcclusion_Forward/URP_AmbientOcclusion
				return;
			}

			universalCameraData.SetRenderer(isMSAAAntiAlias ? 1 : 0); //  URP_Renderer_Forward/URP_Renderer (default)
		}

		void ResetDropdownUI()
		{
			for (int i = 0; i < resolutionList.Count; i++)
			{
				Resolution resolution = resolutionList[i];
				if (!IsSameResolution(resolution, graphicData.ScreenResolution)) continue;

				screenResolutionDropdown.SetCurrentIndex(i);
				currentResolution = resolution;
				break;
			}

			for (int i = 0; i < fullscreenModes.Length; i++)
			{
				if (i != graphicData.ScreenMode) continue;

				screenModeDropdown.SetCurrentIndex(i);
				currentFullScreenMode = i;
				break;
			}

			for (int i = 0; i < qualityNameList.Count; i++)
			{
				string qualityName = qualityNameList[i];

				Func<bool> func = () => (qualityName.Equals(qualityDropdown.StringList[graphicData.Quality]));
				if (ResetDropdown(func, qualityDropdown, ref currentQualityIndex, i)) break;
			}

			int shadowCount = Enum.GetValues(typeof(ShadowResolution)).Length + 1; // +1 for the index 0 (off)
			for (int i = 0; i < shadowCount; i++)
			{
				Func<bool> func = () => (i == graphicData.ShadowResolution);
				if (ResetDropdown(func, shadowDropdown, ref currentShadowIndex, i)) break;
			}

			for (int i = 0; i < antiAliasingDropdown.StringList.Count; i++)
			{
				Func<bool> func = () => (i == graphicData.AntiAliasing);
				if (ResetDropdown(func, antiAliasingDropdown, ref currentAntiAliasIndex, i)) break;
			}
		}

		void ResetToggleUI()
		{
			isVsync.SetCurrentIndex(graphicData.IsVsync ? 1 : 0);
			isVignette.SetCurrentIndex(graphicData.IsVignette ? 1 : 0);
			isDepthOfField.SetCurrentIndex(graphicData.IsDepthOfField ? 1 : 0);
			isMotionBlur.SetCurrentIndex(graphicData.IsMotionBlur ? 1 : 0);
			isBloom.SetCurrentIndex(graphicData.IsBloom ? 1 : 0);
			isAmbientOcclusion.SetCurrentIndex(graphicData.IsAmbientOcclusion ? 1 : 0);
		}

		bool ResetDropdown(Func<bool> condition, UISelectionListing selectionList, ref int toChangeIndex, int replaceWithIndex)
		{
			if (!condition.Invoke()) return false;

			selectionList.SetCurrentIndex(replaceWithIndex);
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
			screenResolutionDropdown.OnValueChanged.RemoveAllListeners();
			screenModeDropdown.OnValueChanged.RemoveAllListeners();

			isVignette.OnValueChanged.RemoveAllListeners();
			isDepthOfField.OnValueChanged.RemoveAllListeners();
			isMotionBlur.OnValueChanged.RemoveAllListeners();
			isBloom.OnValueChanged.RemoveAllListeners();
		}
	}
}