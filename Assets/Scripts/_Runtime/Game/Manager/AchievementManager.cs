using System;
using UnityEngine;

using Steamworks;
using Personal.GameState;
using Personal.Achievement;
using Personal.Save;
using Personal.Character.Player;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.Manager
{
	/// <summary>
	/// Handle achievements.
	/// </summary>
	public class AchievementManager : GameInitializeSingleton<AchievementManager>
	{
		SaveProfile saveProfile;

		PlayerInventory playerInventory;

		protected override void Initialize()
		{
			saveProfile = GameStateBehaviour.Instance.SaveProfile;
		}

		protected override void OnMainScene()
		{
			UnsubscribeEvent();

			playerInventory = StageManager.Instance.PlayerController.Inventory;
			playerInventory.OnUseActiveItem += UseActiveItem;
		}

		void UpdateData(AchievementType achievementType)
		{
			if (saveProfile.UnlockedAchievementList.Contains(achievementType)) return;

			var achievementInfo = MasterDataManager.Instance.Achievement.Get((int)achievementType);
			if (achievementInfo == null)
			{
				Debug.Log("No achievement info for " + achievementType);
				return;
			}

			var targetKey = MasterDataManager.Instance.GetEnumType<Enum>(achievementInfo.targetKey);
			int currentProgress = GlossaryManager.Instance.GetUsedType(targetKey);

			UpdateSteamData(achievementType, currentProgress);

			if (achievementInfo.targetKey < 0 || currentProgress >= achievementInfo.targetRequiredAmount)
			{
				UnlockData(achievementType);
				return;
			}
		}

		public void ResetAll()
		{
			ResetLocalData();
			ResetSteamData();
		}

		void UseActiveItem(Inventory inventory)
		{
			var achievementTypeSet = inventory.PickupableObjectFPS.GetComponentInChildren<AchievementTypeSet>();
			if (achievementTypeSet) UpdateData(achievementTypeSet.AchievementType);
		}

		void UnsubscribeEvent()
		{
			if (!playerInventory) return;
			playerInventory.OnUseActiveItem -= UseActiveItem;
		}

		void OnDestroy()
		{
			UnsubscribeEvent();
		}

		/// -----------------------------------------------------------------------
		/// ------------------------ UPDATE DATA ----------------------------------
		/// -----------------------------------------------------------------------

		void AddSteamData(AchievementType achievementType, int addAmount)
		{
			SteamUserStats.AddStat(achievementType.ToString(), addAmount);
		}

		void UpdateSteamData(AchievementType achievementType, int progress)
		{
			SteamUserStats.SetStat(achievementType.ToString(), progress);
		}

		/// -----------------------------------------------------------------------
		/// --------------------------- UNLOCK ------------------------------------
		/// -----------------------------------------------------------------------

		void UnlockData(AchievementType achievementType)
		{
			saveProfile.AddToAchievement(achievementType);
			UnlockSteamData(achievementType);

			HandleAllAchievement();

			SaveManager.Instance.SaveProfileData();
		}

		void UnlockSteamData(AchievementType achievementType)
		{
			var achievement = new Steamworks.Data.Achievement(achievementType.ToString());
			achievement.Trigger();
		}

		void HandleAllAchievement()
		{
			AchievementType allAchievement = AchievementType.All_Achievements;
			if (saveProfile.UnlockedAchievementList.Contains(allAchievement)) return;

			// Minus 2 because it should not count the Test_Achievement and All_Achievement achievements.
			if (saveProfile.UnlockedAchievementList.Count >= Enum.GetValues(typeof(AchievementType)).Length - 2)
			{
				saveProfile.AddToAchievement(allAchievement);
				UnlockSteamData(allAchievement);
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