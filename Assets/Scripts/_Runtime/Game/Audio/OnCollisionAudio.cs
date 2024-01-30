using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.GameState;

namespace Personal.Setting.Audio
{
	public class OnCollisionAudio : GameInitialize
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.SFX_1;

		[Tooltip("The wait duration before being able to play the next audio. To prevent repetitive playing when colliding with multiple objects.")]
		[SerializeField] float nextWaitBeforeAllow = 0.1f;

		CoroutineRun waitCR = new();

		void OnCollisionEnter(Collision collision)
		{
			if (!waitCR.IsDone) return;

			waitCR = CoroutineHelper.WaitFor(nextWaitBeforeAllow);
			AudioManager.Instance.PlaySFXAt(audioSFXType, collision.GetContact(0).point);
		}
	}
}
