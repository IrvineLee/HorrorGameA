using System;

using Personal.Achievement;
using Personal.Entity;

[Serializable]
public class AchievementEntity : GenericEntity
{
	public string achievementName = "";
	public AchievementType achievementType = AchievementType.Test_Achievement;
	public int ps4ID = 0;
	public int xboxID = 0;
	public string steamID = "";
	public string nameLocalizationKey = "";
	public string descriptionLocalizationKey = "";
	public bool isHidden;
}
