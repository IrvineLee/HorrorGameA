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

		protected virtual async UniTask Awake()
		{
			if (GameManager.Instance == null)
				await UniTask.WaitUntil(() => GameManager.Instance != null);

			if (GameManager.Instance.IsLoadingOver) return;

			enabled = false;
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);

			// Seems like scripts does not get re-enabled the same order as defined in the execution order.
			// Make sure the singleton scripts get initialized first before this script.
			await UniTask.Yield(PlayerLoopTiming.LastUpdate);
			enabled = true;

			AwakeComplete().Forget();
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

		protected virtual void OnUpdate() { }

		async UniTask AwakeComplete()
		{
			await UniTask.NextFrame();
			isAwakeCompleted = true;
		}
	}
}