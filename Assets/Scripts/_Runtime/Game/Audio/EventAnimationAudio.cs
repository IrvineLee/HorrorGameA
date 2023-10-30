using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Setting.Audio
{
	public class EventAnimationAudio : GameInitialize
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.None;

		public void PlaySFX()
		{
			AudioManager.Instance.PlaySFXAt(audioSFXType, transform.position);
		}
	}
}

