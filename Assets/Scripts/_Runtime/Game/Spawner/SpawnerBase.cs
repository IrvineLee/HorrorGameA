using UnityEngine;
using Cysharp.Threading.Tasks;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Spawner
{
	public abstract class SpawnerBase : GameInitialize
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