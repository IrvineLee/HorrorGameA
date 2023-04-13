using UnityEngine;

using Helper;
using Personal.Setting.Audio;
using Personal.Definition;

namespace Personal.Manager
{
	public class AudioManager : MonoBehaviourSingleton<AudioManager>
	{
		[SerializeField] AudioSource bgm = null;
		[SerializeField] AudioSource sfx = null;

		[Space]
		[SerializeField] BGMDefinition BGM_AudioDefinition = null;
		[SerializeField] SFXDefinition SFX_AudioDefinition = null;

		public AudioSource Bgm { get => bgm; }
		public AudioSource Sfx { get => sfx; }

		void Start()
		{
			// TODO : InvalidCastException: Specified cast is not valid.
			//BGM_AudioDefinition.Initialize();
			//SFX_AudioDefinition.Initialize();
		}

		public void PlayBGM(AudioBGMType audioBGMType)
		{
			BGM_AudioDefinition.AudioDictionary.TryGetValue(audioBGMType, out AudioClip audioClip);
			bgm.clip = audioClip;
			bgm.loop = true;
			bgm.Play();
		}

		public void PlaySFX(AudioSFXType audioSFXType)
		{
			SFX_AudioDefinition.AudioDictionary.TryGetValue(audioSFXType, out AudioClip audioClip);
			sfx.clip = audioClip;
			sfx.Play();
		}
	}
}

