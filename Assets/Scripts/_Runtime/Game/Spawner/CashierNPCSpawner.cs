using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

using Personal.Manager;

namespace Personal.Spawner
{
	public class CashierNPCSpawner : SpawnerBase
	{
		void Start()
		{
			StageManager.Instance.RegisterCashierNPCSpawner(this);
		}

		public override async UniTask<GameObject> Spawn(string path)
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
			await UniTask.WaitUntil(() => handle.Status != AsyncOperationStatus.None);

			return handle.Result;
		}
	}
}