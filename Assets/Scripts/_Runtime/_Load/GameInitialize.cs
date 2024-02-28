using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Preloader;

namespace Personal.GameState
{
	/// <summary>
	/// Makes sure the singleton scripts get created first.
	/// </summary>
	public class GameInitialize : MonoBehaviour
	{
		protected bool isInitialized;           // This is normally used for OnTrigger/OnCollision events.

		protected async UniTask Awake()
		{
			if (!Preload.IsLoaded)
			{
				// To prevent any monobehaviour functions from getting called before preload is done.
				bool isInitiallyEnabled = enabled;
				enabled = false;

				await UniTask.WaitUntil(() => Preload.IsLoaded, cancellationToken: this.GetCancellationTokenOnDestroy());
				enabled = isInitiallyEnabled;
			}

			EarlyInitialize();

			// The wait here is to ensure all other GameInitialize scripts calls their own EarlyInitialize() first before Initialize().
			await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());

			Initialize();

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleScene().Forget();

			isInitialized = true;
		}

		void OnEnable()
		{
			if (!Preload.IsLoaded) return;

			OnEnabled();
		}

		void OnDisable()
		{
			if (!Preload.IsLoaded) return;

			OnDisabled();
		}

		/// <summary>
		/// Treat this as a normal Awake function.
		/// </summary>
		protected virtual void EarlyInitialize() { }

		/// <summary>
		/// Treat this as a normal Start function but it still gets called even when script is off. It gets called after OnEnable() and Start().
		/// Gets called after loading persistant data.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// Treat this as OnEnable.
		/// </summary>
		protected virtual void OnEnabled() { }

		/// <summary>
		/// Treat this as OnDisable.
		/// </summary>
		protected virtual void OnDisabled() { }

		/// <summary>
		/// This gets called when scene is in Title scene. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnTitleScene() { }

		/// <summary>
		/// This will get called on the next frame of Initialize.
		/// </summary>
		protected virtual void OnMainScene() { }

		/// <summary>
		/// This will get called after OnMainScene.
		/// </summary>
		protected virtual UniTask OnMainSceneNext() { return UniTask.CompletedTask; }

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleScene().Forget();
		}

		async UniTask HandleScene()
		{
			if (!GameSceneManager.Instance.IsMainScene)
			{
				if (GameSceneManager.Instance.IsScene(SceneName.Title))
					OnTitleScene();

				return;
			}

			// Wait for every GameInitialize script to call its Initialize first before this OnMainScene.
			await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());

			OnMainScene();
			OnMainSceneNext().Forget();
		}

		void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}