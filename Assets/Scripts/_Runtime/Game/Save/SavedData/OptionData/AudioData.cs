using System;
using UnityEngine;

namespace Personal.Setting.Audio
{
	[Serializable]
	public class AudioData
	{
		// Audio
		[SerializeField] float masterVolume = 1;
		[SerializeField] float bgmVolume = 1;
		[SerializeField] float sfxVolume = 1;

		[SerializeField] AudioSpeakerMode audioSpeakerMode = AudioSpeakerMode.Stereo;

		public float MasterVolume { get => masterVolume; set => masterVolume = value; }
		public float BgmVolume { get => bgmVolume; set => bgmVolume = value; }
		public float SfxVolume { get => sfxVolume; set => sfxVolume = value; }
		public AudioSpeakerMode AudioSpeakerMode { get => audioSpeakerMode; set => audioSpeakerMode = value; }

		public void SetVolume(float masterVolume = -1, float bgmVolume = -1, float sfxVolume = -1)
		{
			this.masterVolume = masterVolume == -1 ? this.masterVolume : masterVolume;
			this.bgmVolume = bgmVolume == -1 ? this.bgmVolume : bgmVolume;
			this.sfxVolume = sfxVolume == -1 ? this.sfxVolume : sfxVolume;
		}
	}
}