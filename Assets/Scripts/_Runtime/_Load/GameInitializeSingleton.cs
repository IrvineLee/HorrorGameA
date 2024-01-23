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
			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver, cancellationToken: this.GetCancellationTokenOnDestroy());

			Initialize();

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleScene();

			//Debug.Log("<color=yellow> GameInitializeSingleton " + typeof(T).Name + "</color>");
		}

		/// <summary>
		/// Called right after awake is finished.
		/// </summary>
		protected virtual void Initialize() { }

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
		/// This gets called when scene is in Main scene/scenes. 
		/// </summary>
		/// <returns></returns>
		protected virtual async UniTask OnEarlyMainSceneAsync() { await UniTask.CompletedTask; }

		/// <summary>
		/// This will get called on the next frame of OnEarlyMainScene.
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
			OnEarlyMainSceneAsync().Forget();

			await UniTask.NextFrame();
			OnMainScene();
		}

		void OnApplicationQuit()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}