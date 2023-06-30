using System;
using UnityEngine;

using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

namespace Personal.Setting.Graphic
{
	[Serializable]
	public class GraphicData
	{
		[SerializeField] Resolution screenResolution = default;
		[SerializeField] FullScreenMode screenMode = FullScreenMode.FullScreenWindow;
		[SerializeField] int quality = 5;
		[SerializeField] int shadowResolution = 5;

		[SerializeField] int antiAliasing = 5;
		[SerializeField] bool isVsync = true;

		[SerializeField] bool isVignette = true;
		[SerializeField] bool isDepthOfField = true;
		[SerializeField] bool isMotionBlur = true;
		[SerializeField] bool isBloom = true;
		[SerializeField] bool isAmbientOcclusion = true;

		public Resolution ScreenResolution { get => screenResolution; set => screenResolution = value; }
		public FullScreenMode ScreenMode { get => screenMode; set => screenMode = value; }
		public int Quality { get => quality; set => quality = value; }
		public int ShadowResolution { get => shadowResolution; set => shadowResolution = value; }

		public int AntiAliasing { get => antiAliasing; set => antiAliasing = value; }
		public bool IsVsync { get => isVsync; set => isVsync = value; }

		public bool IsVignette { get => isVignette; set => isVignette = value; }
		public bool IsDepthOfField { get => isDepthOfField; set => isDepthOfField = value; }
		public bool IsMotionBlur { get => isMotionBlur; set => isMotionBlur = value; }
		public bool IsBloom { get => isBloom; set => isBloom = value; }
		public bool IsAmbientOcclusion { get => isAmbientOcclusion; set => isAmbientOcclusion = value; }

		// quality
		// render scale
		// texture quality
		// anisotropic mode
		// anisotropic level

		/// <summary>
		/// Set current resolution on the first time. 
		/// </summary>
		public void HandleFirstTimeUser()
		{
			if (screenResolution.width == 0 && screenResolution.height == 0)
			{
				screenResolution = Screen.currentResolution;
			}
			if (antiAliasing == -1)
			{
				antiAliasing = QualitySettings.antiAliasing;
			}
		}

		public void SetGraphic(Resolution screenResolution, FullScreenMode screenMode, int quality, int shadowResolution, int antiAliasing)
		{
			this.screenResolution = screenResolution;
			this.screenMode = screenMode;
			this.quality = quality;
			this.shadowResolution = shadowResolution;
			this.antiAliasing = antiAliasing;
		}

		public void SetBoolValue(bool isVsync, bool isVignette, bool isDepthOfField, bool isMotionBlur, bool isBloom, bool isAmbientOcclusion)
		{
			this.isVsync = isVsync;
			this.isVignette = isVignette;
			this.isDepthOfField = isDepthOfField;
			this.isMotionBlur = isMotionBlur;
			this.isBloom = isBloom;
			this.isAmbientOcclusion = isAmbientOcclusion;
		}
	}
}