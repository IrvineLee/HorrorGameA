using System;
using UnityEngine;

using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Audio;

namespace Personal.InteractiveObject
{
	public class PhoneHandler : GameInitialize
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.PhoneRing;
		[SerializeField] InteractableObject interactableObject = null;

		protected override void Initialize()
		{
			StageManager.Instance.RegisterPhoneHandler(this);
		}

		public void Ring()
		{
			AudioManager.Instance.PlaySFXLoopAt(audioSFXType, transform.position);
			interactableObject.enabled = true;
		}
	}
}

