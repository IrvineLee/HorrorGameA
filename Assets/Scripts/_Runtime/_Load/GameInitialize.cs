using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using UnityEngine.SceneManagement;

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
			isInitiallyEnabled = enabled;
			enabled = false;

			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			AwakeComplete();
		}

		protected virtual void OnEnable()
		{
			if (!isAwakeCompleted) return;

			OnPostEnable();
		}

		void Update()
		{
			OnUpdate();
		}

		/// <summary>
		/// Gets called right after awake has finished and before getting enabled.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// Gets called after awake has finished and during OnEnable.
		/// </summary>
		protected virtual void OnPostEnable() { }

		/// <summary>
		/// Update
		/// </summary>
		protected virtual void OnUpdate() { }

		/// <summary>
		/// Wait until the scene is in Main scene/scenes before proceeding. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnEarlyMainScene() { }

		/// <summary>
		/// This will get called on the next frame of OnEarlyMainScene.
		/// </summary>
		protected virtual void OnMainScene() { }

		async void AwakeComplete()
		{
			// Wait a frame here so singleton scripts get initialized first before this script.
			await UniTask.NextFrame();

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleMainScene().Forget();

			Initialize();
			if (isInitiallyEnabled) enabled = true;

			isAwakeCompleted = true;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleMainScene().Forget();
		}

		async UniTask HandleMainScene()
		{
			if (!GameSceneManager.Instance.IsMainScene()) return;
			OnEarlyMainScene();

			await UniTask.NextFrame();
			OnMainScene();
		}

		void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}