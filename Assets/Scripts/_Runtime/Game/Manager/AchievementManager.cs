using System;
using UnityEngine;

using Steamworks;
using Personal.GameState;
using Personal.Achievement;
using Personal.Save;

namespace Personal.Manager
{
	/// <summary>
	/// Handle achievements.
	/// </summary>
	public class AchievementManager : GameInitializeSingleton<AchievementManager>
	{
		protected override void Initialize()
		{
			//SteamUserStats.OnAchievementProgress += UnlockSteamData;
		}

		public void AddData(AchievementType achievementType, int addAmount = 1)
		{
			AddLocalData(achievementType, addAmount);
			AddSteamData(achievementType, addAmount);
		}

		public void UpdateData(AchievementType achievementType, int currentProgress)
		{
			UpdateLocalData(achievementType, currentProgress);
			UpdateSteamData(achievementType, currentProgress);
		}

		public void Unlock(AchievementType achievementType)
		{
			UnlockLocalData(achievementType);
			UnlockSteamData(achievementType);
		}

		public void ResetAll()
		{
			ResetLocalData();
			ResetSteamData();
		}

		/// -----------------------------------------------------------------------
		/// ------------------------ UPDATE DATA ----------------------------------
		/// -----------------------------------------------------------------------

		void AddLocalData(AchievementType achievementType, int addAmount)
		{

		}

		void AddSteamData(AchievementType achievementType, int addAmount)
		{
			Steamworks.SteamUserStats.AddStat(achievementType.ToString(), addAmount);
		}

		void UpdateLocalData(AchievementType achievementType, int progress)
		{

		}

		void UpdateSteamData(AchievementType achievementType, int progress)
		{
			SteamUserStats.SetStat(achievementType.ToString(), progress);
		}

		/// -----------------------------------------------------------------------
		/// --------------------------- UNLOCK ------------------------------------
		/// -----------------------------------------------------------------------

		void UnlockLocalData(AchievementType achievementType)
		{
			var saveProfile = GameStateBehaviour.Instance.SaveProfile;

			bool isAdded = saveProfile.AddToAchievement(achievementType);
			if (!isAdded) return;

			HandleLocalAllAchievement(saveProfile);
			SaveManager.Instance.SaveProfileData();
		}

		void UnlockSteamData(AchievementType achievementType)
		{
			var achievement = new Steamworks.Data.Achievement(achievementType.ToString());
			achievement.Trigger();
		}

		//void UnlockSteamData(Steamworks.Data.Achievement achievement, int currentProgress, int max)
		//{
		//	if (!achievement.State) return;

		//	Debug.Log($"{achievement.Name} WAS UNLOCKED!");
		//}

		void HandleLocalAllAchievement(SaveProfile saveProfile)
		{
			if (saveProfile.UnlockedAchievementList.Contains(AchievementType.All_Achievements)) return;

			// Minus 2 because it should not count the Test_Achievement and All_Achievement achievements.
			if (saveProfile.UnlockedAchievementList.Count >= Enum.GetValues(typeof(AchievementType)).Length - 2)
			{
				UnlockLocalData(AchievementType.All_Achievements);
			}
		}

		/// -----------------------------------------------------------------------
		/// ------------------------- RESET DATA ----------------------------------
		/// -----------------------------------------------------------------------

		void ResetLocalData()
		{
			var saveProfile = GameStateBehaviour.Instance.SaveProfile;
			saveProfile.ResetAchievement();

			SaveManager.Instance.SaveProfileData();
		}

		void ResetSteamData()
		{
			SteamUserStats.ResetAll(true); // true = wipe achivements too
			SteamUserStats.StoreStats();
			SteamUserStats.RequestCurrentStats();
		}
	}
}