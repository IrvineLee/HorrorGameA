using System;

using Personal.Entity;
using Personal.Setting.Audio;

[Serializable]
public class SFXEntity : GenericEntity
{
	public string name;
	public AudioSFXType audioSFXType;
}
