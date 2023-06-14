using UnityEngine;

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

			await UniTask.WaitUntil(() => GameSceneManager.Instance && GameSceneManager.Instance.IsMainScene());
			OnMainScene();

			//Debug.Log("<color=yellow> GameInitializeSingleton " + typeof(T).Name + "</color>");
		}

		/// <summary>
		/// Called right after awake is finished.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// Wait until the scene is in Main scene/scenes before proceeding. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnMainScene() { }
	}
}