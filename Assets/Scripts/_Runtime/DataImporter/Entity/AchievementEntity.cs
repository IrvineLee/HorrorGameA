using System;

using Personal.Entity;

[Serializable]
public class AchievementEntity : GenericEntity
{
	public string achievementName = "";
	public int ps4ID = 0;
	public int xboxID = 0;
	public string steamID = "";
	public string nameLocalizationKey = "";
	public bool isHidden;
}
