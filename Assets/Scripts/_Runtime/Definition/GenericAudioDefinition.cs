using System;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace Personal.Definition
{
	[Serializable]
	public class GenericAudioDefinition<T> : ScriptableObject where T : Enum
	{
		[Serializable]
		public class Audio
		{
			[SerializeField] T audioType;
			[SerializeField] AudioClip audioClip;

			public T AudioType { get => audioType; }
			public AudioClip AudioClip { get => audioClip; }

			public Audio(T audioType, AudioClip audioClip)
			{
				this.audioType = audioType;
				this.audioClip = audioClip;
			}
		}

		[SerializeField] List<Audio> audioList = new();

#if UNITY_EDITOR
		public List<Audio> AudioList { get => audioList; }
#endif
		public IReadOnlyDictionary<T, AudioClip> AudioDictionary { get => dictionary; }

		Dictionary<T, AudioClip> dictionary = new();

		/// <summary>
		/// Initialize the dictionary of audio.
		/// </summary>
		public void Initialize()
		{
			dictionary.Clear();
			foreach (var audio in audioList)
			{
				dictionary.Add(audio.AudioType, audio.AudioClip);
			}
		}
	}
}