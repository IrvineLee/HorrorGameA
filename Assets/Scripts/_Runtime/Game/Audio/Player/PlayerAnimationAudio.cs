using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.Player;
using Personal.Manager;
using Personal.Setting.Audio;

namespace Personal.Character.Animation
{
	public class PlayerAnimationAudio : AnimatorController
	{
		[SerializeField] List<AudioSFXType> footstepSFXTypes = new List<AudioSFXType>();
		[SerializeField] AudioSFXType landSFXType = AudioSFXType.PlayerLand;

		[SerializeField] float footstepVolume = 0.5f;

		CharacterController controller;

		protected async override void Initialize()
		{
			// Have to wait for FPS controller to get it's controller.
			await UniTask.DelayFrame(1);

			controller = StageManager.Instance.PlayerController.FSM.FPSController.Controller;
		}

		void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight <= 0.5f) return;
			if (footstepSFXTypes.Count <= 0) return;

			var index = Random.Range(0, footstepSFXTypes.Count);
			AudioManager.Instance.PlaySFXOnceAt(footstepSFXTypes[index], transform.TransformPoint(controller.center), footstepVolume);
		}

		void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight <= 0.5f) return;
			AudioManager.Instance.PlaySFXOnceAt(landSFXType, transform.TransformPoint(controller.center), footstepVolume);
		}
	}
}