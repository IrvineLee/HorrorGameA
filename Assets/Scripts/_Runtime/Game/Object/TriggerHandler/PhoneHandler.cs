using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class PhoneHandler : GameInitialize
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.PhoneRing;
		[SerializeField] InteractableObject interactableObject = null;

		public void Ring()
		{
			AudioManager.Instance.PlaySFXAt(audioSFXType, transform.position, isLoop: true);
			interactableObject.enabled = true;
		}
	}
}

