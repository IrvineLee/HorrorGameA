using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Setting.Audio;

namespace Personal.InteractiveObject
{
	public class AudioSFXTriggerHandler : EventTriggerHandler
	{
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.None;
		[SerializeField] Transform audioAtTrans = null;

		protected override UniTask<bool> HandleTrigger()
		{
			if (!IsInteractable) return new UniTask<bool>(false);

			if (audioAtTrans)
			{
				AudioManager.Instance.PlaySFXAt(audioSFXType, audioAtTrans.position);
			}
			else
			{
				AudioManager.Instance.PlaySFX(audioSFXType);
			}

			colliderTrans.gameObject.SetActive(false);
			return new UniTask<bool>(true);
		}
	}
}

