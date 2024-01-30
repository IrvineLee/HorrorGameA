using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Personal.Setting.Audio;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class AudioState : StateBase
	{
		[SerializeField] AudioSourceType audioSourceType = AudioSourceType.SFX;

		[ShowIf("@audioSourceType == AudioSourceType.BGM")]
		[SerializeField] AudioBGMType audioBGMType = AudioBGMType.BGM_Boss_01;

		[ShowIf("@audioSourceType == AudioSourceType.SFX")]
		[SerializeField] AudioSFXType audioSFXType = AudioSFXType.SFX_1;

		[ShowIf("@audioSourceType == AudioSourceType.SFX")]
		[SerializeField] Transform target = null;

		[SerializeField] float startDelay = 0;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			await UniTask.Delay(startDelay.SecondsToMilliseconds());

			if (audioSourceType == AudioSourceType.BGM) AudioManager.Instance.PlayBGM(audioBGMType);
			else if (audioSourceType == AudioSourceType.SFX)
			{
				if (target) AudioManager.Instance.PlaySFXAt(audioSFXType, target.position);
				else AudioManager.Instance.PlaySFX(audioSFXType);
			}
		}
	}
}