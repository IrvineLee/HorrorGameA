using System;
using UnityEngine;

using Helper;
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
		public static event Action<string, int> OnAchievementAddEvent;
		public static event Action<string, int> OnAchievementSetEvent;
		public static event Action<string> OnAchievementUnlockEvent;
		public static event Action OnResetEvent;

		SaveProfile saveProfile;
		PlayerInventory playerInventory;

		protected override void Initialize()
		{
			saveProfile = GameStateBehaviour.Instance.SaveProfile;
		}

		protected override void OnMainScene()
		{
			UnsubscribeEvent();

			// You will always want the GlossaryManager to subscribe and update first before this.
			CoroutineHelper.WaitNextFrame(() =>
			{
				playerInventory = StageManager.Instance.PlayerController.Inventory;
				playerInventory.OnUseActiveItem += UseActiveItem;
			});
		}

		/// <summary>
		/// Call this to unlock the achievement.
		/// </summary>
		/// <param name="achievementType"></param>
		public void Unlock(AchievementType achievementType)
		{
			if (saveProfile.UnlockedAchievementList.Contains(achievementType)) return;

			UnlockData(achievementType);
		}

		public void ResetAll()
		{
			ResetLocalData();
			ResetSteamData();
		}

		/// <summary>
		/// This will always use the FPS-view item, NOT the pickupable that you pick up in the scene.
		/// </summary>
		/// <param name="inventory"></param>
		void UseActiveItem(Inventory inventory)
		{
			var achievementTypeSet = inventory.PickupableObjectFPS.GetComponentInChildren<AchievementTypeSet>();
			if (achievementTypeSet) UpdateData(achievementTypeSet.AchievementType);
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

			var targetKeyType = MasterDataManager.Instance.GetEnumType<Enum>(achievementInfo.targetKey);
			int currentProgress = GlossaryManager.Instance.GetUsedType(targetKeyType);

			SetSteamData(achievementType, currentProgress);

			if (achievementInfo.targetKey < 0 || currentProgress >= achievementInfo.targetRequiredAmount)
			{
				UnlockData(achievementType);
				return;
			}
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
		/// ------------------------ STEAM DATA ----------------------------------
		/// -----------------------------------------------------------------------

		void AddSteamData(AchievementType achievementType, int value = 1)
		{
			OnAchievementAddEvent?.Invoke(achievementType.ToString(), value);
		}

		void SetSteamData(AchievementType achievementType, int value)
		{
			OnAchievementSetEvent?.Invoke(achievementType.ToString(), value);
		}

		void UnlockSteamData(AchievementType achievementType)
		{
			OnAchievementUnlockEvent?.Invoke(achievementType.ToString());
		}

		void ResetSteamData()
		{
			OnResetEvent?.Invoke();
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

		void HandleAllAchievement()
		{
			AchievementType allAchievement = AchievementType.AllAchievements;
			if (saveProfile.UnlockedAchievementList.Contains(allAchievement)) return;

			// Minus 1 because it should not count the All_Achievement achievements.
			if (saveProfile.UnlockedAchievementList.Count >= Enum.GetValues(typeof(AchievementType)).Length - 1)
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
	}
}