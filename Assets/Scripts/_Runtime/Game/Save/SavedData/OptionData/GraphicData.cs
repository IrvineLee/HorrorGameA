using System;
using UnityEngine;

namespace Personal.Setting.Graphic
{
	[Serializable]
	public class GraphicData
	{
		[SerializeField] float brightness = 0;

		[SerializeField] Resolution screenResolution = default;
		[SerializeField] FullScreenMode screenMode = FullScreenMode.FullScreenWindow;

		[SerializeField] int antiAliasing = 0;
		[SerializeField] bool isVsync = true;

		[SerializeField] bool isVignette = false;
		[SerializeField] bool isDepthOfField = false;
		[SerializeField] bool isMotionBlur = false;
		[SerializeField] bool isBloom = false;
		[SerializeField] bool isAmbientOcclusion = false;

		public float Brightness { get => brightness; set => brightness = value; }

		public Resolution ScreenResolution { get => screenResolution; set => screenResolution = value; }
		public FullScreenMode ScreenMode { get => screenMode; set => screenMode = value; }

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

		public void SetResolutionAndScreenMode(Resolution screenResolution, FullScreenMode screenMode)
		{
			this.screenResolution = screenResolution;
			this.screenMode = screenMode;
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