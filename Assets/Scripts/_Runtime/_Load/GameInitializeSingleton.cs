using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Helper;

namespace Personal.GameState
{
	public class GameInitializeSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour
	{
		protected bool isBootCompleted;

		protected override async UniTask Boot()
		{
			enabled = false;

			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			Initialize();

			isBootCompleted = true;
			enabled = true;

			SceneManager.sceneLoaded += OnSceneLoaded;
			HandleMainScene().Forget();

			//Debug.Log("<color=yellow> GameInitializeSingleton " + typeof(T).Name + "</color>");
		}

		void Update()
		{
			if (!isBootCompleted) return;

			OnUpdate();
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
		/// Typically used when GameInitialize has to start initializing first before this script.
		/// </summary>
		protected virtual void OnPostMainScene() { }

		/// <summary>
		/// Update after boot is completed.
		/// </summary>
		protected virtual void OnUpdate() { }

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			HandleMainScene().Forget();
		}

		async UniTask HandleMainScene()
		{
			if (!GameSceneManager.Instance.IsMainScene()) return;

			if (!isBootCompleted)
				await UniTask.WaitUntil(() => isBootCompleted);

			OnEarlyMainScene();

			await UniTask.NextFrame();
			OnMainScene();

			await UniTask.NextFrame();
			OnPostMainScene();
		}

		void OnApplicationQuit()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}