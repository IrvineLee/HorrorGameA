using System;
using UnityEngine;

using Personal.Setting.Audio;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "BGMDefinition", menuName = "ScriptableObjects/Audio/BGMDefinition", order = 0)]
	[Serializable]
	public class BGMDefinition : GenericAudioDefinition<AudioBGMType>
	{
	}
}