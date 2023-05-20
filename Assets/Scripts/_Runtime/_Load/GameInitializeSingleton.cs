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
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);
			enabled = true;

			//Debug.Log("<color=yellow> GameInitializeSingleton " + typeof(T).Name + "</color>");
		}
	}
}