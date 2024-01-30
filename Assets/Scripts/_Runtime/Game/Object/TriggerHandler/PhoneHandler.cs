using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class PhoneHandler : GameInitialize
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.SFX_1;
		[SerializeField] KeyEventType endKeyEventType = KeyEventType.None;

		public void Ring()
		{
			AudioManager.Instance.PlaySFXAt(audioSFXType, transform.position, isLoop: true);
			StageManager.Instance.RegisterKeyEvent(endKeyEventType);
		}
	}
}

