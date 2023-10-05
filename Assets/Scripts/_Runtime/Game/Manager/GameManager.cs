using UnityEngine;

using Helper;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		public static bool IsLoadingOver { get; private set; }

		public static bool IsWindow { get => Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor; }

		protected override async UniTask Boot()
		{
			IsLoadingOver = false;

			await UniTask.WaitUntil(() => IsInitialized());
			Debug.Log("<Color=#45FF00> ---------- All MANAGERS successfully initiated!! ----------</color>");

			await HandleLoading();
			Debug.Log("<Color=#45FF00> ---------- Profile Loaded!! ----------</color>");

			// Initialize after data loading.
			MasterDataManager.Initialize();

			IsLoadingOver = true;
		}

		bool IsInitialized()
		{
			if (AudioManager.Instance == null) return false;
			if (CursorManager.Instance == null) return false;
			if (DebugManager.Instance == null) return false;
			if (GameSceneManager.Instance == null) return false;
			if (GameStateBehaviour.Instance == null) return false;
			if (InputManager.Instance == null) return false;
			if (PoolManager.Instance == null) return false;
			if (RumbleManager.Instance == null) return false;
			if (SaveManager.Instance == null) return false;
			if (SteamManager.Instance == null) return false;
			if (StageManager.Instance == null) return false;
			if (UIManager.Instance == null) return false;
			if (HelperObj.Instance == null) return false;

			MasterDataManager.CreateInstance();
			if (MasterDataManager.Instance == null) return false;

			return true;
		}

		async UniTask HandleLoading()
		{
			SaveManager.Instance.LoadProfileData();
			SaveManager.Instance.LoadSlotData();

			await UniTask.DelayFrame(10, cancellationToken: this.GetCancellationTokenOnDestroy());

			SaveManager.Instance.SaveProfileData();
		}
	}
}