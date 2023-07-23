using UnityEngine;

using Personal.GameState;
using System;
using Personal.Achievement;
using Personal.Save;

namespace Personal.Manager
{
	/// <summary>
	/// Handle achievements.
	/// </summary>
	public class AchievementManager : GameInitializeSingleton<AchievementManager>
	{
		public void Unlock(AchievementType achievementType)
		{
			var saveProfile = GameStateBehaviour.Instance.SaveProfile;

			bool isAdded = saveProfile.AddToAchievement(achievementType);
			if (!isAdded) return;

			HandleAllAchievement(saveProfile);
			SaveManager.Instance.SaveProfileData();
		}

		public void ResetAll()
		{
			var saveProfile = GameStateBehaviour.Instance.SaveProfile;
			saveProfile.ResetAchievement();

			SaveManager.Instance.SaveProfileData();
		}

		void HandleAllAchievement(SaveProfile saveProfile)
		{
			if (saveProfile.UnlockedAchievementList.Contains(AchievementType.All_Achievements)) return;

			// Minus 2 because it should not count the Test_Achievement and All_Achievement achievements.
			if (saveProfile.UnlockedAchievementList.Count >= Enum.GetValues(typeof(AchievementType)).Length - 2)
			{
				Unlock(AchievementType.All_Achievements);
			}
		}
	}
}

