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

		protected void OnEnable()
		{
			if (!isAwakeCompleted) return;

			OnPostEnable();
		}

		protected void OnDisable()
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

		async void AwakeComplete()
		{
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
			if (!GameSceneManager.Instance.IsMainScene()) return;

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