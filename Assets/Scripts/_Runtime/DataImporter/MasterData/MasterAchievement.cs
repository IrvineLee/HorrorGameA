using System;
using System.Collections.Generic;

using Helper;
using Personal.Achievement;
using Personal.Data;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterAchievement : MasterGeneric<AchievementEntity, AchievementType>
{
	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<AchievementType, AchievementEntity>();

		foreach (var entity in Entities)
		{
			dictionary.Add(entity.achievementType, entity);
		}
	}

	/// <summary>
	/// Get achievement data from MasterData.
	/// </summary>
	/// <param name="achievementType"></param>
	/// <returns></returns>
	public override AchievementEntity Get(AchievementType achievementType)
	{
		var result = Dictionary.GetOrDefault(achievementType);
		return result;
	}
}
