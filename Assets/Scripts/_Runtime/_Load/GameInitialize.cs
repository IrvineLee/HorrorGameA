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
			enabled = true;
		}

		protected virtual void Update() { }
	}
}