using UnityEngine;

using Helper;
using Steamworks;
using Cysharp.Threading.Tasks;

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

		void OnDestroy()
		{
			SteamClient.Shutdown();
		}
	}
}