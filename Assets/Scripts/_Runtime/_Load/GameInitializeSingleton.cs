using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Helper;

namespace Personal.GameState
{
	public class GameInitializeSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour
	{
		protected override async UniTask Boot()
		{
			enabled = false;

			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			Initialize();
			InitializeUniTask().Forget();

			enabled = true;

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleScene();

			//Debug.Log("<color=yellow> GameInitializeSingleton " + typeof(T).Name + "</color>");
		}

		/// <summary>
		/// Called right after awake is finished.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// UniTask where it is called right after awake is finished.
		/// </summary>
		protected virtual async UniTask InitializeUniTask()
		{
			await UniTask.Yield(PlayerLoopTiming.LastInitialization);
		}

		/// <summary>
		/// This gets called when scene is in Title scene. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnTitleScene() { }

		/// <summary>
		/// This gets called when scene is in Main scene/scenes. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnEarlyMainScene() { }

		/// <summary>
		/// This will get called on the next frame of OnEarlynMainScene.
		/// </summary>
		protected virtual void OnMainScene() { }

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleScene();
		}

		void HandleScene()
		{
			if (!GameSceneManager.Instance.IsMainScene())
			{
				if (GameSceneManager.Instance.IsScene(SceneName.Title))
					OnTitleScene();

				return;
			}

			HandleMainScene().Forget();
		}

		async UniTask HandleMainScene()
		{
			OnEarlyMainScene();

			await UniTask.NextFrame();
			OnMainScene();
		}

		void OnApplicationQuit()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}