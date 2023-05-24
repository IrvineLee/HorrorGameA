using System;
using UnityEngine;

using Personal.Setting.Audio;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "SFXDefinition", menuName = "ScriptableObjects/Audio/SFXDefinition", order = 0)]
	[Serializable]
	public class SFXDefinition : GenericAudioDefinition<AudioSFXType>
	{
	}
}