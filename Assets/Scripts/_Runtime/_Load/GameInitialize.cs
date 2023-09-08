using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Preloader;
using Personal.Constant;
using Helper;

namespace Personal.GameState
{
	/// <summary>
	/// Makes sure the singleton scripts get created first.
	/// </summary>
	public class GameInitialize : MonoBehaviour
	{
		bool isInitiallyLoaded;

		protected async UniTask Awake()
		{
			isInitiallyLoaded = enabled;

			if (!Preload.IsLoaded)
			{
				// To prevent any monobehaviour functions from getting called before preload is done.
				enabled = false;

				await UniTask.WaitUntil(() => Preload.IsLoaded, cancellationToken: this.GetCancellationTokenOnDestroy());
			}

			// Wait for the singleton scripts to handle its OnSceneLoaded first before initializing this script.
			//await UniTask.Yield(PlayerLoopTiming.LastInitialization);

			enabled = isInitiallyLoaded;
			Initialize();

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleScene().Forget();
		}

		/// <summary>
		/// Treat this as a normal Awake function.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// This gets called when scene is in Title scene. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnTitleScene() { }

		/// <summary>
		/// This will get called on the next frame of Initialize.
		/// </summary>
		protected virtual void OnMainScene() { }

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