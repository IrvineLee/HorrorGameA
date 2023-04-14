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

		/// <summary>
		/// Set the volume. Put to -1 for no change.
		/// </summary>
		/// <param name="masterVolume"></param>
		/// <param name="bgmVolume"></param>
		/// <param name="sfxVolume"></param>
		public void SetVolume(float masterVolume = -1, float bgmVolume = -1, float sfxVolume = -1)
		{
			this.masterVolume = masterVolume == -1 ? this.masterVolume : masterVolume;
			this.bgmVolume = bgmVolume == -1 ? this.bgmVolume : bgmVolume;
			this.sfxVolume = sfxVolume == -1 ? this.sfxVolume : sfxVolume;
		}

		/// <summary>
		/// Set audio speaker mode.
		/// </summary>
		/// <param name="audioSpeakerMode"></param>
		public void SetAudioSpeakerMode(AudioSpeakerMode audioSpeakerMode)
		{
			this.audioSpeakerMode = audioSpeakerMode;
			Debug.Log("Saved as " + this.audioSpeakerMode);
		}
	}
}