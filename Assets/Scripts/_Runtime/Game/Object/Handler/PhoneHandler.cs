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
			AudioManager.Instance.PlaySFXLoopAt(audioSFXType, transform.position);
			interactableObject.enabled = true;
		}
	}
}

