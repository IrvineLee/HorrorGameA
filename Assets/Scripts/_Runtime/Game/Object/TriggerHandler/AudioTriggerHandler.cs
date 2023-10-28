using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;

namespace Personal.InteractiveObject
{
	public class AudioTriggerHandler : InteractableObject
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.None;

		void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;

			AudioManager.Instance.PlaySFX(audioSFXType);

			// Disable the trigger collider.
			currentCollider.enabled = false;
		}
	}
}

