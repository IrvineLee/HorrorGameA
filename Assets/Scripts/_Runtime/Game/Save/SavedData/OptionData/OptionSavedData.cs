using UnityEngine;
using System;

using Personal.Setting.Graphic;
using Personal.Setting.Audio;
using Personal.Setting.Control;
using Personal.Setting.Language;

namespace Personal.Save
{
	[Serializable]
	public class OptionSavedData
	{
		[SerializeField] GraphicData graphicData = new GraphicData();
		[SerializeField] AudioData audioData = new AudioData();
		[SerializeField] ControlData controlData = new ControlData();
		[SerializeField] LanguageData languageData = new LanguageData();

		public GraphicData GraphicData { get => graphicData; set => graphicData = value; }
		public AudioData AudioData { get => audioData; set => audioData = value; }
		public ControlData ControlData { get => controlData; set => controlData = value; }
		public LanguageData LanguageData { get => languageData; set => languageData = value; }
	}
}