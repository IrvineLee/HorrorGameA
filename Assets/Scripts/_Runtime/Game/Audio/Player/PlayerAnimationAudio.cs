using System.Collections.Generic;
using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.Character.Player;
using Personal.Definition;
using Personal.Setting.Audio;

namespace Personal.Character.Animation
{
	public class PlayerAnimationAudio : AnimatorAudio
	{
		[SerializeField] AnimationAudioDefinition animationAudioDefinition = null;
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

		List<AudioSFXType> GetFootstepSFXList(AreaFootstepSFXType areaFootstepSFXType)
		{
			switch (areaFootstepSFXType)
			{
				case AreaFootstepSFXType.Grass: if (animationAudioDefinition.GrassFootstepList.Count == 0) break; return animationAudioDefinition.GrassFootstepList;
			}

			return animationAudioDefinition.GenericFootstepList;
		}

		List<AudioSFXType> GetLandSFXList(AreaFootstepSFXType areaFootstepSFXType)
		{
			switch (areaFootstepSFXType)
			{
				case AreaFootstepSFXType.Grass: if (animationAudioDefinition.GrassLandList.Count == 0) break; return animationAudioDefinition.GrassLandList;
			}

			return animationAudioDefinition.GenericLandList;
		}

		AudioSFXType GetSFXType(List<AudioSFXType> audioList)
		{
			var index = Random.Range(0, audioList.Count);
			return audioList[index];
		}
	}
}