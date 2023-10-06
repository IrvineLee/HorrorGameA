using UnityEngine;
using UnityEngine.Audio;

using Helper;
using Personal.Setting.Audio;
using Personal.Definition;
using Personal.GameState;

namespace Personal.Manager
{
	public class AudioManager : GameInitializeSingleton<AudioManager>
	{
		[SerializeField] AudioSource bgm = null;
		[SerializeField] AudioSource sfx = null;

		[Space]
		[SerializeField] AudioMixerGroup bgmMixerGroup = null;
		[SerializeField] AudioMixerGroup sfxMixerGroup = null;

		[Space]
		[SerializeField] BGMDefinition BGM_AudioDefinition = null;
		[SerializeField] SFXDefinition SFX_AudioDefinition = null;

		public AudioSource Bgm { get => bgm; }
		public AudioSource Sfx { get => sfx; }

		void Start()
		{
			BGM_AudioDefinition.Initialize();
			SFX_AudioDefinition.Initialize();
		}

		public void SetBGMVolume(float value01)
		{
			SetMixerVolume(bgmMixerGroup, "BGM", value01);
		}

		public void SetSFXVolume(float value01)
		{
			SetMixerVolume(sfxMixerGroup, "SFX", value01);
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

		/// <summary>
		/// Play sfx once at position.
		/// </summary>
		/// <param name="audioSFXType"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		/// <param name="isLoop"></param>
		/// <returns></returns>
		public AudioSource PlaySFXAt(AudioSFXType audioSFXType, Vector3 position, float volume = -1, bool isLoop = false)
		{
			volume = volume == -1 ? sfx.volume : volume;
			return SetupSFX(audioSFXType, position, volume, isLoop);
		}

		/// <summary>
		/// Setup the sfx with it's audio, position and other settings.
		/// </summary>
		/// <param name="audioSFXType"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		/// <param name="isLoop"></param>
		/// <returns></returns>
		AudioSource SetupSFX(AudioSFXType audioSFXType, Vector3 position, float volume, bool isLoop)
		{
			SFX_AudioDefinition.AudioDictionary.TryGetValue(audioSFXType, out AudioClip audioClip);

			if (!audioClip) return null;

			GameObject go = PoolManager.Instance.GetSpawnedObject(audioSFXType.ToString());
			AudioSource audioSource = go?.GetComponent<AudioSource>(); ;

			if (go)
			{
				go.transform.position = position;
				SetSFX(audioSource, audioClip, isLoop, volume);

				return audioSource;
			}

			go = new GameObject(audioSFXType.ToString());
			go.transform.position = position;

			audioSource = go.AddComponent<AudioSource>();
			SetSFX(audioSource, audioClip, isLoop, volume);

			return audioSource;
		}

		void SetSFX(AudioSource audioSource, AudioClip audioClip, bool isLoop, float volume)
		{
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.outputAudioMixerGroup = sfxMixerGroup;
			audioSource.Play();

			if (isLoop)
			{
				audioSource.loop = true;
				return;
			}

			CoroutineHelper.WaitFor(audioSource.clip.length, () =>
			{
				if (!audioSource) return;
				PoolManager.Instance.ReturnSpawnedObject(audioSource.gameObject);
			});
		}

		void SetMixerVolume(AudioMixerGroup audioMixerGroup, string str, float value01)
		{
			if (value01 > 1) value01 = 1;

			// Reason why you multiply by 20. https://en.wikipedia.org/wiki/Decibel#Uses
			float value = Mathf.Log10(value01) * 20;
			if (value01 <= 0) value = -80;

			audioMixerGroup.audioMixer.SetFloat(str, value);
		}
	}
}

