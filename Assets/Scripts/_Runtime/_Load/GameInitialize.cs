using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Preloader;

namespace Personal.GameState
{
	/// <summary>
	/// Makes sure the singleton scripts get created first.
	/// Update function will only run after Awake is done.
	/// </summary>
	public class GameInitialize : MonoBehaviour
	{
		bool isInitiallyEnabled = true;

		protected async UniTask Awake()
		{
			if (!Preload.IsLoaded)
				await UniTask.WaitUntil(() => Preload.IsLoaded, cancellationToken: this.GetCancellationTokenOnDestroy());

			isInitiallyEnabled = enabled;
			enabled = false;

			AwakeComplete();
		}

		void Update()
		{
			OnUpdate();
		}

		/// <summary>
		/// Gets called 1-frame after awake has finished and before getting enabled.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// Update
		/// </summary>
		protected virtual void OnUpdate() { }

		/// <summary>
		/// This will get called on the next frame of Initialize.
		/// </summary>
		protected virtual void OnMainScene() { }

		/// <summary>
		/// This will be called when returning to Title scene.
		/// </summary>
		protected virtual void OnTitleScene() { }

		async void AwakeComplete()
		{
			// Wait a frame here so singleton scripts get initialized first before this script.
			await UniTask.NextFrame();

			Initialize();
			if (isInitiallyEnabled) enabled = true;

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleMainScene().Forget();
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleMainScene().Forget();
		}

		async UniTask HandleMainScene()
		{
			if (!GameSceneManager.Instance.IsMainScene())
			{
				if (GameSceneManager.Instance.IsScene(SceneName.Title))
					OnTitleScene();

				return;
			}

			// Reason for loop timing is to make sure other GameInitialize script call its Initialize first before this OnMainScene.
			await UniTask.NextFrame(PlayerLoopTiming.LastTimeUpdate);
			OnMainScene();
		}

		void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}