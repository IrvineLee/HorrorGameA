using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.Achievement;

namespace Personal.Save
{
	/// <summary>
	/// This saves the general info of the user.
	/// </summary>
	[Serializable]
	public class SaveProfile : GenericSave
	{
		[SerializeField] OptionSavedData optionSavedData = new OptionSavedData();

		public OptionSavedData OptionSavedData { get => optionSavedData; }
		public List<AchievementType> UnlockedAchievementList { get; } = new List<AchievementType>();

		public bool AddToAchievement(AchievementType achievementType)
		{
			if (UnlockedAchievementList.Contains(achievementType)) return false;

			UnlockedAchievementList.Add(achievementType);
			return true;
		}

		public void ResetAchievement()
		{
			UnlockedAchievementList.Clear();
		}
	}
}
