using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Setting.Audio
{
	public class PlayAnimationAudio : GameInitialize
	{
		[SerializeField] AudioBGMType audioBGMType = AudioBGMType.None;
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.None;

		public void PlayBGM()
		{
			AudioManager.Instance.PlayBGM(audioBGMType);
		}

		public void PlaySFX()
		{
			AudioManager.Instance.PlaySFXAt(audioSFXType, transform.position);
		}
	}
}

