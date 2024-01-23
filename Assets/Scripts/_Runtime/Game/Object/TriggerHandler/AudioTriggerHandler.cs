using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Setting.Audio;

namespace Personal.InteractiveObject
{
	public class AudioTriggerHandler : EventTriggerHandler
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.None;

		protected override UniTask<bool> HandleTrigger()
		{
			if (!IsInteractable) return new UniTask<bool>(false);

			AudioManager.Instance.PlaySFX(audioSFXType);
			colliderTrans.gameObject.SetActive(false);

			return new UniTask<bool>(true);
		}
	}
}

