using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.Setting.Audio;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "AnimationAudioDefinition", menuName = "ScriptableObjects/Audio/AnimationAudioDefinition", order = 0)]
	[Serializable]
	public class AnimationAudioDefinition : ScriptableObject
	{
		[SerializeField] List<AudioSFXType> genericFootstepList = new();
		[SerializeField] List<AudioSFXType> grassFootstepList = new();

		[SerializeField] List<AudioSFXType> genericLandList = new();
		[SerializeField] List<AudioSFXType> grassLandList = new();

		public List<AudioSFXType> GenericFootstepList { get => genericFootstepList; }
		public List<AudioSFXType> GrassFootstepList { get => grassFootstepList; }

		public List<AudioSFXType> GenericLandList { get => genericLandList; }
		public List<AudioSFXType> GrassLandList { get => grassLandList; }
	}
}