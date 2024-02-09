using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;
using Personal.GameState;
using Personal.KeyEvent;

namespace Personal.InteractiveObject
{
	public class PhoneHandler : GameInitialize
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.None;
		[SerializeField] KeyEventType endKeyEventType = KeyEventType.None;

		public void Ring()
		{
			AudioManager.Instance.PlaySFXAt(audioSFXType, transform.position, isLoop: true);
			StageManager.Instance.RegisterKeyEvent(endKeyEventType);
		}
	}
}

