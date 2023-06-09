using UnityEngine;

using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.Spawner
{
	public abstract class SpawnerBase : MonoBehaviour
	{
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