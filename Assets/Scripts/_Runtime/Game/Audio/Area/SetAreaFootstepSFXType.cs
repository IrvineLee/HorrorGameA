using UnityEngine;

using Personal.GameState;
using Personal.Character.Animation;

namespace Personal.Setting.Audio
{
	public class SetAreaFootstepSFXType : GameInitialize
	{
		[SerializeField] AreaFootstepSFXType areaFootstepSFXType = AreaFootstepSFXType.None;

		HumanAnimationAudio humanAnimatorAudio;

		void OnTriggerEnter(Collider other)
		{
			humanAnimatorAudio = other.GetComponentInChildren<HumanAnimationAudio>();
			humanAnimatorAudio?.SetAreaFootstep(areaFootstepSFXType);
		}

		void OnTriggerExit(Collider other)
		{
			humanAnimatorAudio?.SetAreaFootstep(AreaFootstepSFXType.None);
		}
	}
}
