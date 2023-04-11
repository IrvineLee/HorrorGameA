using UnityEngine;
using System;

namespace Personal.Save
{
	[Serializable]
	public class SettingSavedData
	{
		[SerializeField] int bgmVolume = 1;
		[SerializeField] int sfxVolume = 1;

		public int BgmVolume { get => bgmVolume; set => bgmVolume = value; }
		public int SfxVolume { get => sfxVolume; set => sfxVolume = value; }
	}
}