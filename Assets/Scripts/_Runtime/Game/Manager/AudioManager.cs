using UnityEngine;

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
		[SerializeField] BGMDefinition BGM_AudioDefinition = null;
		[SerializeField] SFXDefinition SFX_AudioDefinition = null;

		public AudioSource Bgm { get => bgm; }
		public AudioSource Sfx { get => sfx; }

		void Start()
		{
			BGM_AudioDefinition.Initialize();
			SFX_AudioDefinition.Initialize();
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
		/// <returns></returns>
		public AudioSource PlaySFXOnceAt(AudioSFXType audioSFXType, Vector3 position, float volume = -1)
		{
			volume = volume == -1 ? sfx.volume : volume;
			return SetupSFX(audioSFXType, position, false, volume);
		}

		/// <summary>
		/// Play sfx loop at position.
		/// </summary>
		/// <param name="audioSFXType"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		/// <returns></returns>
		public AudioSource PlaySFXLoopAt(AudioSFXType audioSFXType, Vector3 position, float volume = -1)
		{
			volume = volume == -1 ? sfx.volume : volume;
			return SetupSFX(audioSFXType, position, true, volume);
		}

		/// <summary>
		/// Setup the sfx with it's audio, position and other settings.
		/// </summary>
		/// <param name="audioSFXType"></param>
		/// <param name="position"></param>
		/// <param name="isLoop"></param>
		/// <param name="volume"></param>
		/// <returns></returns>
		AudioSource SetupSFX(AudioSFXType audioSFXType, Vector3 position, bool isLoop, float volume)
		{
			SFX_AudioDefinition.AudioDictionary.TryGetValue(audioSFXType, out AudioClip audioClip);

			if (!audioClip) return null;

			GameObject go = PoolManager.Instance.GetSpawnedObject(audioSFXType.GetStringValue());
			AudioSource audioSource = null;

			if (go)
			{
				go.transform.position = position;

				audioSource = go.GetComponent<AudioSource>();
				SetSFX(audioSource, audioClip, isLoop, volume);

				return audioSource;
			}

			go = new GameObject(audioSFXType.GetStringValue());
			go.transform.position = position;

			audioSource = go.AddComponent<AudioSource>();
			SetSFX(audioSource, audioClip, isLoop, volume);

			return audioSource;
		}

		void SetSFX(AudioSource audioSource, AudioClip audioClip, bool isLoop, float volume)
		{
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.Play();

			if (isLoop)
			{
				audioSource.loop = true;
				return;
			}

			CoroutineHelper.WaitFor(audioSource.clip.length, () =>
			{
				PoolManager.Instance.ReturnSpawnedObject(audioSource.gameObject);
			});
		}
	}
}

