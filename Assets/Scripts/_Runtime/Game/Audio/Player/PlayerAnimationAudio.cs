using System.Collections.Generic;
using UnityEngine;

using Personal.Manager;
using Personal.Setting.Audio;
using Personal.Character.Player;
using Helper;

namespace Personal.Character.Animation
{
	public class PlayerAnimationAudio : AnimatorController
	{
		[SerializeField] List<AudioSFXType> footstepSFXTypes = new List<AudioSFXType>();
		[SerializeField] AudioSFXType landSFXType = AudioSFXType.PlayerLand;

		[SerializeField] float footstepWeight = 0.02f;
		[SerializeField] float footstepVolume = 0.5f;

		FPSController fpsController;
		CoroutineRun SEDelayCR = new();

		protected override void Awake()
		{
			fpsController = StageManager.Instance.PlayerController.FPSController;
		}

		/// <summary>
		/// Animation event.
		/// </summary>
		/// <param name="animationEvent"></param>
		void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight <= footstepWeight) return;
			if (footstepSFXTypes.Count <= 0) return;
			if (!SEDelayCR.IsDone) return;

			var index = Random.Range(0, footstepSFXTypes.Count);
			AudioManager.Instance.PlaySFXAt(footstepSFXTypes[index], transform.TransformPoint(fpsController.Controller.center), footstepVolume);
			SEDelayCR = CoroutineHelper.WaitFor(0.15f);
		}

		/// <summary>
		/// Animation event.
		/// </summary>
		/// <param name="animationEvent"></param>
		void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight <= 0.5f) return;
			AudioManager.Instance.PlaySFXAt(landSFXType, transform.TransformPoint(fpsController.Controller.center), footstepVolume);
		}
	}
}