using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Helper;

namespace Personal.GameState
{
	public class GameInitializeSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour
	{
		protected override async UniTask Awake()
		{
			await base.Awake();

			if (GameManager.Instance == null)
				await UniTask.WaitUntil(() => GameManager.Instance != null);

			enabled = false;
			await UniTask.WaitUntil(() => GameManager.Instance && GameManager.Instance.IsLoadingOver);
			enabled = true;

			Initialize();

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleMainScene();

			//Debug.Log("<color=yellow> GameInitializeSingleton " + typeof(T).Name + "</color>");
		}

		/// <summary>
		/// Called right after awake is finished.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// This gets called when scene is in Main scene/scenes. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnEarlyMainScene() { }

		/// <summary>
		/// This will get called on the next frame of OnEarlynMainScene.
		/// </summary>
		protected virtual void OnMainScene() { }

		/// <summary>
		/// This will get called on the next frame of OnMainScene.
		/// </summary>
		protected virtual void OnPostMainScene() { }

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleMainScene();
		}

		async void HandleMainScene()
		{
			if (!GameSceneManager.Instance.IsMainScene()) return;
			OnEarlyMainScene();

			await UniTask.NextFrame();
			OnMainScene();

			await UniTask.NextFrame();
			OnPostMainScene();
		}

		void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}