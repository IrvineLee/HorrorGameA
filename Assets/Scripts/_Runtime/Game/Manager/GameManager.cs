using System.Collections;
using UnityEngine;

using Helper;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }
		public bool IsLoadingOver { get; private set; }

		public static bool IsWindow { get => Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor; }
		public static bool IsMAC { get => Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor; }

		async void Start()
		{
			MasterDataManager.CreateInstance();
			await UniTask.WaitUntil(() => IsInitialized());

			Debug.Log("<Color=#45FF00> ---------- All MANAGERS successfully activated!! ----------</color>");

			await HandleProfileLoading();

			IsLoadingOver = true;
		}

		bool IsInitialized()
		{
			if (GameStateBehaviour.Instance == null) return false;
			if (SceneManager.Instance == null) return false;
			if (UIManager.Instance == null) return false;
			if (PoolManager.Instance == null) return false;
			if (StageManager.Instance == null) return false;
			if (DebugManager.Instance == null) return false;
			if (SaveManager.Instance == null) return false;

			if (MasterDataManager.Instance == null) return false;

			return true;
		}

		async UniTask HandleProfileLoading()
		{
			SaveManager.Instance.LoadProfileData();
			await UniTask.DelayFrame(10);

			// To make sure the profile gets created the 1st time around.
			SaveManager.Instance.SaveProfileData();
		}
	}
}