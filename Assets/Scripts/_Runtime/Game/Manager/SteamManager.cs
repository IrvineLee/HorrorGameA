using System;
using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;
using Steamworks;
using Steamworks.Data;

namespace Personal.Manager
{
	public class SteamManager : MonoBehaviourSingleton<SteamManager>
	{
		public bool IsClientValid { get => SteamClient.IsValid; }
		public string PlayerName { get => IsClientValid ? SteamClient.Name : ""; }

		// STEAM : Replace this with the correct app id.
		//uint appID = 252490;

		protected override UniTask Boot()
		{
			//try
			//{
			//	SteamClient.Init(appID);
			//}
			//catch
			//{
			//	Debug.Log("Steam is closed? Can't find steam_api dll? Don't have permission to play app?");
			//}

			return base.Boot();
		}

		void Start()
		{
			AchievementManager.OnAchievementAddEvent += AddProgress;
			AchievementManager.OnAchievementSetEvent += SetProgress;
			AchievementManager.OnAchievementUnlockEvent += UnlockAchievement;
			AchievementManager.OnResetEvent += ResetData;
		}

		void Update()
		{
			SteamClient.RunCallbacks();
		}

		public async UniTask<Image?> GetAvatar()
		{
			if (!IsClientValid) return null;

			try
			{
				// Get Avatar using await
				return await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
			}
			catch (Exception e)
			{
				// If something goes wrong, log it
				Debug.Log(e);
				return null;
			}
		}

		/// <summary>
		/// Add to achievement value.
		/// </summary>
		/// <param name="achievementStr"></param>
		/// <param name="value"></param>
		void AddProgress(string achievementStr, int value = 1)
		{
			if (!IsClientValid) return;
			SteamUserStats.AddStat(achievementStr, value);
		}

		/// <summary>
		/// Set the achievement value.
		/// </summary>
		/// <param name="achievementStr"></param>
		/// <param name="value"></param>
		void SetProgress(string achievementStr, int value)
		{
			if (!IsClientValid) return;
			SteamUserStats.SetStat(achievementStr, value);
		}

		/// <summary>
		/// Unlock an achievement.
		/// </summary>
		/// <param name="achievementStr"></param>
		void UnlockAchievement(string achievementStr)
		{
			if (!IsClientValid) return;

			var achievement = new Steamworks.Data.Achievement(achievementStr);
			achievement.Trigger();
		}

		void ResetData()
		{
			if (!IsClientValid) return;

			SteamUserStats.ResetAll(true); // true = wipe achivements too
			SteamUserStats.StoreStats();
			SteamUserStats.RequestCurrentStats();
		}

		void OnDestroy()
		{
			SteamClient.Shutdown();

			AchievementManager.OnAchievementAddEvent -= AddProgress;
			AchievementManager.OnAchievementSetEvent -= SetProgress;
			AchievementManager.OnAchievementUnlockEvent -= UnlockAchievement;
			AchievementManager.OnResetEvent -= ResetData;
		}
	}
}