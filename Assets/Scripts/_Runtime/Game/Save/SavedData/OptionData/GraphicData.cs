using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Setting.Graphic
{
	[Serializable]
	public class GraphicData
	{
		[SerializeField] int brightness = 1;

		[SerializeField] Resolution screenResolution = default;
		[SerializeField] FullScreenMode screenMode = FullScreenMode.FullScreenWindow;

		//[SerializeField] TMP_Dropdown antiAliasingDropdown = null;
		[SerializeField] bool isVsync = true;

		[SerializeField] bool isVignette = true;
		[SerializeField] bool isDepthOfField = true;
		[SerializeField] bool isMotionBlur = true;
		[SerializeField] bool isBloom = true;
		[SerializeField] bool isAmbientOcclusion = true;

		public int Brightness { get => brightness; set => brightness = value; }
		public Resolution ScreenResolution { get => screenResolution; set => screenResolution = value; }
		public FullScreenMode ScreenMode { get => screenMode; set => screenMode = value; }
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

		public void SetBoolValue(bool isVsync, bool isVignette, bool isDepthOfField, bool isMotionBlur, bool isBloom, bool isAmbientOcclusion)
		{
			this.isVsync = isVsync;
			this.isVignette = isVignette;
			this.isDepthOfField = isDepthOfField;
			this.isMotionBlur = isMotionBlur;
			this.isBloom = isBloom;
			this.isAmbientOcclusion = isAmbientOcclusion;
		}
		//Screen.resolutions
		//// Graphic
		//[SerializeField] Resolution resolution = Screen.currentResolution;
	}

}