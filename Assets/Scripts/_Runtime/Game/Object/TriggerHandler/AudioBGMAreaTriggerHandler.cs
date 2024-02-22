using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class AudioBGMAreaTriggerHandler : EventTriggerHandler
	{
		AudioSource audioSource;

		protected override void Initialize()
		{
			base.Initialize();
			audioSource = GetComponentInChildren<AudioSource>(true);
		}

		protected override UniTask HandleTriggerEnter(Collider other)
		{
			AudioManager.Instance.PlayAreaBGM(true, audioSource);
			return UniTask.CompletedTask;
		}

		protected override UniTask HandleTriggerExit(Collider other)
		{
			AudioManager.Instance.PlayAreaBGM(false, audioSource);
			return UniTask.CompletedTask;
		}
	}
}

