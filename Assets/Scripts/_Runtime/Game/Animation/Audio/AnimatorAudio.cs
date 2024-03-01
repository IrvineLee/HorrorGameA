using System;
using UnityEngine;

using Personal.Definition;

namespace Personal.Character.Animation
{
	public abstract class AnimatorAudio : AnimatorController
	{
		[SerializeField] protected AnimationAudioDefinition animationAudioDefinition = null;
	}
}