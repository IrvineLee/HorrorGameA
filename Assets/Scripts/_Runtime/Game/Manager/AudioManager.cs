using UnityEngine;
using System.Collections.Generic;

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
		[SerializeField] GenericAudioDefinition<AudioBGMType> BGM_AudioDefinition = null;

		void Start()
		{
			BGM_AudioDefinition.Initialize();
		}

		public void PlayBGM(AudioBGMType audioBGMType)
		{
			//bgm.clip = audioClip;
			//bgm.loop = true;
			//bgm.Play();
		}

		public void PlaySFX(AudioClip audioClip)
		{
			sfx.clip = audioClip;
			sfx.Play();
		}
	}
}

