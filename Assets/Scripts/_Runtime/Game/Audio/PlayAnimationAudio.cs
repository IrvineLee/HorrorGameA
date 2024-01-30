using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Setting.Audio
{
	public class PlayAnimationAudio : GameInitialize
	{
		[SerializeField] AudioBGMType audioBGMType = AudioBGMType.BGM_Boss_01;
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.SFX_1;

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

