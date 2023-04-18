using System.Collections;
using UnityEngine;

using Helper;
using Personal.GameState;

namespace Personal.Manager
{
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }
		public bool IsLoadingOver { get; private set; }

		IEnumerator Start()
		{
			MasterDataManager.CreateInstance();
			yield return new WaitUntil(() => IsInitialized());

			Debug.Log("<Color=#45FF00> ---------- All MANAGERS successfully activated!! ----------</color>");

			yield return HandleProfileLoading();

			IsLoadingOver = true;
		}

		bool IsInitialized()
		{
			if (GameStateBehaviour.Instance == null) return false;
			if (SceneManager.Instance == null) return false;
			if (UIManager.Instance == null) return false;
			if (PoolManager.Instance == null) return false;
			if (DebugManager.Instance == null) return false;
			if (SaveManager.Instance == null) return false;

			if (MasterDataManager.Instance == null) return false;

			return true;
		}

		IEnumerator HandleProfileLoading()
		{
			SaveManager.Instance.LoadProfileData();
			yield return null;

			// To make sure the profile gets created the 1st time around.
			SaveManager.Instance.SaveProfileData();
		}
	}
}