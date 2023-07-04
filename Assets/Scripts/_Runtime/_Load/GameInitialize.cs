using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Preloader;

namespace Personal.GameState
{
	/// <summary>
	/// Makes sure the singleton scripts get created first.
	/// You might wanna use 'isAwakeCompleted' in derived classes to check whether awake has been completed.
	/// Update function will only run after Awake is done.
	/// </summary>
	public class GameInitialize : MonoBehaviour
	{
		protected bool isAwakeCompleted;

		bool isInitiallyEnabled = true;

		protected async UniTask Awake()
		{
			if (!Preload.IsLoaded)
				await UniTask.WaitUntil(() => Preload.IsLoaded, cancellationToken: this.GetCancellationTokenOnDestroy());

			isInitiallyEnabled = enabled;
			enabled = false;

			AwakeComplete();
		}

		protected virtual void OnEnable()
		{
			if (!isAwakeCompleted) return;

			OnPostEnable();
		}

		protected virtual void OnDisable()
		{
			if (!isAwakeCompleted) return;

			OnPostDisable();
		}

		void Update()
		{
			OnUpdate();
		}

		/// <summary>
		/// Gets called right after awake has finished and before getting enabled.
		/// </summary>
		protected virtual void PreInitialize() { }

		/// <summary>
		/// Gets called 1-frame after awake has finished and before getting enabled.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// Gets called after awake has finished and during OnEnable.
		/// </summary>
		protected virtual void OnPostEnable() { }

		/// <summary>
		/// Gets called after awake has finished and during OnDisable.
		/// </summary>
		protected virtual void OnPostDisable() { }

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
			PreInitialize();

			// Wait a frame here so singleton scripts get initialized first before this script.
			await UniTask.NextFrame();

			Initialize();
			if (isInitiallyEnabled) enabled = true;

			isAwakeCompleted = true;

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