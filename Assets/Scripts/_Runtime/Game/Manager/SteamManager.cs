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

		void Update()
		{
			SteamClient.RunCallbacks();
		}

		public async UniTask<Image?> GetAvatar()
		{
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

		void OnDestroy()
		{
			SteamClient.Shutdown();
		}
	}
}