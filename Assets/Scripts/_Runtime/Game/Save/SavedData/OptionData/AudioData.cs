using System;
using UnityEngine;

namespace Personal.Setting.Audio
{
	[Serializable]
	public class AudioData
	{
		// Audio
		[SerializeField] int masterVolume = 1;
		[SerializeField] int bgmVolume = 1;
		[SerializeField] int sfxVolume = 1;

		[SerializeField] AudioSpeakerMode audioSpeakerMode = AudioSpeakerMode.Stereo;

		public int MasterVolume { get => masterVolume; set => masterVolume = value; }
		public int BgmVolume { get => bgmVolume; set => bgmVolume = value; }
		public int SfxVolume { get => sfxVolume; set => sfxVolume = value; }
		public AudioSpeakerMode AudioSpeakerMode { get => audioSpeakerMode; set => audioSpeakerMode = value; }
	}
}