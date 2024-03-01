using System.Collections.Generic;
using UnityEngine;

using Personal.Setting.Audio;

namespace Personal.Character.Animation
{
	public class HumanAnimationAudio : AnimatorAudio
	{
		protected AreaFootstepSFXType areaFootstepSFXType = AreaFootstepSFXType.None;

		public void SetAreaFootstep(AreaFootstepSFXType areaFootstepSFXType) { this.areaFootstepSFXType = areaFootstepSFXType; }

		protected virtual void OnFootstep(AnimationEvent animationEvent) { }
		protected virtual void OnLand(AnimationEvent animationEvent) { }

		protected List<AudioSFXType> GetFootstepSFXList(AreaFootstepSFXType areaFootstepSFXType)
		{
			switch (areaFootstepSFXType)
			{
				case AreaFootstepSFXType.Grass: if (animationAudioDefinition.GrassFootstepList.Count == 0) break; return animationAudioDefinition.GrassFootstepList;
			}

			return animationAudioDefinition.GenericFootstepList;
		}

		protected List<AudioSFXType> GetLandSFXList(AreaFootstepSFXType areaFootstepSFXType)
		{
			switch (areaFootstepSFXType)
			{
				case AreaFootstepSFXType.Grass: if (animationAudioDefinition.GrassLandList.Count == 0) break; return animationAudioDefinition.GrassLandList;
			}

			return animationAudioDefinition.GenericLandList;
		}

		protected AudioSFXType GetSFXType(List<AudioSFXType> audioList)
		{
			var index = Random.Range(0, audioList.Count);
			return audioList[index];
		}
	}
}