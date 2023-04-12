using System;
using Personal.Entity;
using Personal.Setting.Audio;

[Serializable]
public class BGMEntity : GenericEntity
{
	public string name;
	public AudioBGMType audioBGMType;
}
