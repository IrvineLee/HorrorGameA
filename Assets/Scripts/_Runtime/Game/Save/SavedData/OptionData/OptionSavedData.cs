using UnityEngine;
using System;

using Personal.Setting.Graphic;
using Personal.Setting.Audio;
using Personal.Setting.Control;
using Personal.Setting.Game;

namespace Personal.Save
{
	[Serializable]
	public class OptionSavedData
	{
		[SerializeField] GameData gameData = new GameData();
		[SerializeField] GraphicData graphicData = new GraphicData();
		[SerializeField] AudioData audioData = new AudioData();
		[SerializeField] ControlData controlData = new ControlData();

		public GameData GameData { get => gameData; }
		public GraphicData GraphicData { get => graphicData; }
		public AudioData AudioData { get => audioData; }
		public ControlData ControlData { get => controlData; }

		public void ResetGameData() { gameData = new GameData(); }
		public void ResetGraphicData() { graphicData = new GraphicData(); }
		public void ResetAudioData() { audioData = new AudioData(); }
		public void ResetControlData() { controlData = new ControlData(); }
	}
}