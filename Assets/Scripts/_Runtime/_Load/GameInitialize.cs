using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

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
			if (GameManager.Instance == null)
				await UniTask.WaitUntil(() => GameManager.Instance != null);

			// Disable all scripts when starting so singleton could initialize first.
			isInitiallyEnabled = enabled;
			enabled = false;

			if (GameManager.Instance.IsLoadingOver)
			{
				AwakeComplete();
				return;
			}

			await UniTask.WaitUntil(() => GameManager.Instance && GameManager.Instance.IsLoadingOver);
			AwakeComplete();
		}

		protected virtual async UniTask OnEnable()
		{
			if (isAwakeCompleted) return;
			await UniTask.WaitUntil(() => isAwakeCompleted);
		}

		void Update()
		{
			if (StageManager.Instance.IsPaused) return;

			OnUpdate();
		}

		/// <summary>
		/// Gets called right after awake has finished and before getting enabled.
		/// </summary>
		protected virtual void Initialize() { }

		/// <summary>
		/// Update
		/// </summary>
		protected virtual void OnUpdate() { }

		/// <summary>
		/// Wait until the scene is in Main scene/scenes before proceeding. 
		/// </summary>
		/// <returns></returns>
		protected virtual void OnMainScene() { }

		async void AwakeComplete()
		{
			// Since some singleton scripts only get initalized when entering a scene,
			// wait a frame here so singleton scripts get initialized first before this script.
			await UniTask.NextFrame();

			isAwakeCompleted = true;

			Initialize();
			if (isInitiallyEnabled) enabled = true;

			await UniTask.WaitUntil(() => GameSceneManager.Instance.IsMainScene());
			OnMainScene();
		}
	}
}