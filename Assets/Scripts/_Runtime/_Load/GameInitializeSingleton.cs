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

			enabled = false;
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);
			enabled = true;
		}
	}
}