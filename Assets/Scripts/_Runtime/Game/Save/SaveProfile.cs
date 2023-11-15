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
		// The latest save slot data. Used for continue button on the title scene. As of now there's only 1 save slot.
		[SerializeField] int latestSaveSlot = -1;

		[SerializeField] OptionSavedData optionSavedData = new OptionSavedData();
		[SerializeField] List<AchievementType> unlockedAchievementList = new();

		public int LatestSaveSlot { get => latestSaveSlot; set => latestSaveSlot = value; }
		public OptionSavedData OptionSavedData { get => optionSavedData; }
		public List<AchievementType> UnlockedAchievementList { get => unlockedAchievementList; }

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
