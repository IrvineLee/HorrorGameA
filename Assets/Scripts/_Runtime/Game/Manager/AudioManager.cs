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

		public void PlaySFXOnceAt(AudioSFXType audioSFXType, Vector3 position, float volume)
		{
			GameObject go = PoolManager.Instance.GetSpawnedObject(audioSFXType.GetStringValue());

			AudioSource audioSource = null;
			if (go)
			{
				go.transform.position = position;

				audioSource = go.GetComponent<AudioSource>();
				SetSFX(audioSource, volume);

				return;
			}

			SFX_AudioDefinition.AudioDictionary.TryGetValue(audioSFXType, out AudioClip audioClip);

			go = new GameObject(audioSFXType.GetStringValue());
			go.transform.position = position;

			audioSource = go.AddComponent<AudioSource>();
			audioSource.clip = audioClip;
			SetSFX(audioSource, volume);
		}

		void SetSFX(AudioSource audioSource, float volume)
		{
			audioSource.volume = volume;
			audioSource.Play();

			CoroutineHelper.WaitFor(audioSource.clip.length, () =>
			{
				PoolManager.Instance.ReturnSpawnedObject(audioSource.gameObject);
			});
		}
	}
}

