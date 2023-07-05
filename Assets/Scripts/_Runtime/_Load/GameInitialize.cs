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
		protected async UniTask Awake()
		{
			if (!Preload.IsLoaded)
				await UniTask.WaitUntil(() => Preload.IsLoaded, cancellationToken: this.GetCancellationTokenOnDestroy());

			// Wait for the singleton scripts to handle its OnSceneLoaded first before initializing this script.
			await UniTask.Yield(PlayerLoopTiming.LastInitialization);

			Initialize();

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleScene().Forget();
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

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleScene().Forget();
		}

		async UniTask HandleScene()
		{
			if (!GameSceneManager.Instance.IsMainScene())
			{
				if (GameSceneManager.Instance.IsScene(SceneName.Title))
					OnTitleScene();

				return;
			}

			// Wait for every GameInitialize script to call its Initialize first before this OnMainScene.
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);
			OnMainScene();
		}

		void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}