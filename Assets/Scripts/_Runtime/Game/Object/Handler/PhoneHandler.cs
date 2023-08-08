using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;

namespace Personal.InteractiveObject
{
	public class PhoneHandler : MonoBehaviour
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.PhoneRing;
		[SerializeField] InteractableObject interactableObject = null;

		void Awake()
		{
			StageManager.Instance.RegisterPhoneHandler(this);
		}

		public void Ring()
		{
			AudioManager.Instance.PlaySFXAt(audioSFXType, transform.position, isLoop: true);
			interactableObject.enabled = true;
		}
	}
}

