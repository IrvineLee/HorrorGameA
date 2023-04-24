using UnityEngine;
using Cysharp.Threading.Tasks;

using Personal.Manager;

namespace Personal.Spawner
{
	public abstract class SpawnerBase : MonoBehaviour
	{
		protected virtual async UniTask Start()
		{
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);
		}

		protected abstract UniTask<GameObject> Spawn(string path, Vector3 position = default);
	}
}