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
		public static bool IsMAC { get => Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor; }

		protected override async UniTask Boot()
		{
			IsLoadingOver = false;

			await UniTask.WaitUntil(() => IsInitialized());
			Debug.Log("<Color=#45FF00> ---------- All MANAGERS successfully initiated!! ----------</color>");

			await HandleProfileLoading();
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
			if (StageManager.Instance == null) return false;
			if (UIManager.Instance == null) return false;
			if (HelperObj.Instance == null) return false;

			MasterDataManager.CreateInstance();
			if (MasterDataManager.Instance == null) return false;

			return true;
		}

		async UniTask HandleProfileLoading()
		{
			bool isNewlyCreated = SaveManager.Instance.LoadProfileData();
			await UniTask.DelayFrame(10, cancellationToken: this.GetCancellationTokenOnDestroy());

			if (!isNewlyCreated) return;

			// To make sure the profile gets created the 1st time around.
			// You might wanna re-save it if new data are added to the save profile after releasing it.
			SaveManager.Instance.SaveProfileData();
		}
	}
}