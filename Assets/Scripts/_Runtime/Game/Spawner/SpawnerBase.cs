using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Personal.Spawner
{
	public abstract class SpawnerBase : MonoBehaviour
	{
		public abstract UniTask<GameObject> Spawn(string path);
	}
}