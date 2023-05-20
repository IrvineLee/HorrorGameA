using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.GameState
{
	public class GameInitialize : MonoBehaviour
	{
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
		}

		protected virtual void Update() { }
	}
}