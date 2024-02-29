using System;
using UnityEngine;

using Personal.Setting.Audio;

namespace Personal.Character.Animation
{
	public abstract class AnimatorAudio : AnimatorController
	{
		protected AreaFootstepSFXType areaFootstepSFXType = AreaFootstepSFXType.None;

		public void SetAreaFootstep(AreaFootstepSFXType areaFootstepSFXType) { this.areaFootstepSFXType = areaFootstepSFXType; }

		protected virtual void OnFootstep(AnimationEvent animationEvent) { }
		protected virtual void OnLand(AnimationEvent animationEvent) { }
	}
}