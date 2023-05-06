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

		/// <summary>
		/// Handle the spawning of objects.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected virtual async UniTask<GameObject> Spawn(string path, Vector3 position = default)
		{
			return await AddressableHelper.Spawn(path, position);
		}
	}
}