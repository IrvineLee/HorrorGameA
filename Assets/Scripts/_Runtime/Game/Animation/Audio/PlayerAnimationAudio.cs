using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.Character.Animation
{
	public class PlayerAnimationAudio : HumanAnimationAudio
	{
		[SerializeField] float footstepWeight = 0.02f;

		FPSController fpsController;
		CoroutineRun SEDelayCR = new();

		protected override void Initialize()
		{
			fpsController = StageManager.Instance.PlayerController.FPSController;
		}

		/// <summary>
		/// Animation event.
		/// </summary>
		/// <param name="animationEvent"></param>
		protected override void OnFootstep(AnimationEvent animationEvent)
		{
			// Surprisingly timeline will call this even in editor mode.
			if (!Application.isPlaying) return;

			if (animationAudioDefinition == null) return;
			if (animationEvent.animatorClipInfo.weight <= footstepWeight) return;
			if (!SEDelayCR.IsDone) return;

			var footstepList = GetFootstepSFXList(areaFootstepSFXType);
			var SFXType = GetSFXType(footstepList);

			AudioManager.Instance.PlaySFXAt(SFXType, transform.TransformPoint(fpsController.Controller.center));
			SEDelayCR = CoroutineHelper.WaitFor(0.15f);
		}

		/// <summary>
		/// Animation event.
		/// </summary>
		/// <param name="animationEvent"></param>
		protected override void OnLand(AnimationEvent animationEvent)
		{
			if (animationAudioDefinition == null) return;
			if (animationEvent.animatorClipInfo.weight <= 0.5f) return;

			var landList = GetLandSFXList(areaFootstepSFXType);
			var SFXType = GetSFXType(landList);

			AudioManager.Instance.PlaySFXAt(SFXType, transform.TransformPoint(fpsController.Controller.center));
		}
	}
}