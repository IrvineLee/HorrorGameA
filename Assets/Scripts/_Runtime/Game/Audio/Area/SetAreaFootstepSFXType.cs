using UnityEngine;

using Personal.GameState;
using Personal.Character.Animation;

namespace Personal.Setting.Audio
{
	public class SetAreaFootstepSFXType : GameInitialize
	{
		[SerializeField] AreaFootstepSFXType areaFootstepSFXType = AreaFootstepSFXType.None;

		AnimatorAudio animatorAudio;

		void OnTriggerEnter(Collider other)
		{
			animatorAudio = other.GetComponentInChildren<AnimatorAudio>();
			animatorAudio?.SetAreaFootstep(areaFootstepSFXType);
		}

		void OnTriggerExit(Collider other)
		{
			animatorAudio?.SetAreaFootstep(AreaFootstepSFXType.None);
		}
	}
}
