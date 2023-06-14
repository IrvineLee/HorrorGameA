using System.Collections.Generic;
using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;
using Personal.Character.Player;

namespace Personal.Character.Animation
{
	public class PlayerAnimationAudio : AnimatorController
	{
		[SerializeField] List<AudioSFXType> footstepSFXTypes = new List<AudioSFXType>();
		[SerializeField] AudioSFXType landSFXType = AudioSFXType.PlayerLand;

		[SerializeField] float footstepVolume = 0.5f;

		FPSController fpsController;

		protected override void OnPostMainScene()
		{
			fpsController = StageManager.Instance.PlayerController.FPSController;
		}

		/// <summary>
		/// Animation event.
		/// </summary>
		/// <param name="animationEvent"></param>
		void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight <= 0.5f) return;
			if (footstepSFXTypes.Count <= 0) return;

			var index = Random.Range(0, footstepSFXTypes.Count);
			AudioManager.Instance.PlaySFXOnceAt(footstepSFXTypes[index], transform.TransformPoint(fpsController.Controller.center), footstepVolume);
		}

		/// <summary>
		/// Animation event.
		/// </summary>
		/// <param name="animationEvent"></param>
		void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight <= 0.5f) return;
			AudioManager.Instance.PlaySFXOnceAt(landSFXType, transform.TransformPoint(fpsController.Controller.center), footstepVolume);
		}
	}
}