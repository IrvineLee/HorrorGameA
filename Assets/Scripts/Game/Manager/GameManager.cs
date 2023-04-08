using System.Collections;
using UnityEngine;

using Helper;
using Personal.CameraView;

namespace Personal.Manager
{
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }

		IEnumerator Start()
		{
			MasterDataManager.CreateInstance();
			yield return new WaitUntil(() => IsInitialized());
		}

		bool IsInitialized()
		{
			if (SceneManager.Instance == null) return false;
			if (DebugManager.Instance == null) return false;

			if (MasterDataManager.Instance == null) return false;

			return true;
		}
	}
}