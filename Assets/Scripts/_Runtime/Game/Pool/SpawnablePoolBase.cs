using Cysharp.Threading.Tasks;
using Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Personal.Pool
{
	public class SpawnablePoolBase : MonoBehaviour
	{
		[SerializeField] PoolType poolType = PoolType.Effect3D_Effect01;
		[SerializeField] AssetReference assetReference = null;

		[Space]
		[SerializeField] int initialSpawn = 5;
		[SerializeField] int defaultCapacity = 10;
		[SerializeField] int maxPoolSize = 50;

		public PoolType PoolType { get => poolType; }

		ObjectPool<GameObject> pool;
		string key = "";

		void Start()
		{
			key = poolType.GetStringValue().SearchFrontRemoveEnd('.', true);
			Initialize();
		}

		/// <summary>
		/// Get the object from the pool.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="parent"></param>
		/// <param name="worldPositionStays"></param>
		/// <returns></returns>
		public virtual GameObject Get(Vector3 position = default, Transform parent = null, bool worldPositionStays = true)
		{
			var go = pool.Get();
			go.transform.position = position;
			go.transform.SetParent(parent, worldPositionStays);

			return go;
		}

		/// <summary>
		/// Return the object to the pool.
		/// </summary>
		/// <param name="go"></param>
		public void Return(GameObject go)
		{
			pool.Release(go);
		}

		/// <summary>
		/// Make sure it's loaded, setup the pool and initial spawn it.
		/// </summary>
		async void Initialize()
		{
			bool isLoaded = await LoadAddressable();

			if (!isLoaded)
				Debug.Log(key + " is not loaded properly.");

			pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
			await InitalSpawn();
		}

		async UniTask<bool> LoadAddressable()
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
			await UniTask.WaitUntil(() => handle.Status != AsyncOperationStatus.None, cancellationToken: this.GetCancellationTokenOnDestroy());

			return handle.Status == AsyncOperationStatus.Succeeded;
		}

		async UniTask InitalSpawn()
		{
			List<GameObject> goList = new();
			for (int i = 0; i < initialSpawn; i++)
			{
				goList.Add(Get());
			}

			await UniTask.DelayFrame(1);

			for (int i = 0; i < goList.Count; i++)
			{
				goList[i].SetActive(false);
			}
		}

		GameObject CreatePooledItem()
		{
			// Since it was already pre-loaded, this will always instantiate.
			var go = assetReference.InstantiateAsync(transform).Result;
			go.GetComponentInChildren<SpawnableObject>()?.SetPool(this);

			return go;
		}

		protected virtual void OnTakeFromPool(GameObject pooledObj)
		{
			pooledObj.gameObject.SetActive(true);
		}

		protected virtual async void OnReturnedToPool(GameObject pooledObj)
		{
			pooledObj.gameObject.SetActive(false);
			await Task.Yield();
			pooledObj.transform.SetParent(transform);
		}

		protected virtual void OnDestroyPoolObject(GameObject pooledObj)
		{
			Addressables.ReleaseInstance(pooledObj.gameObject);
		}

		void OnValidate()
		{
			transform.name = "Pool_" + poolType;
		}
	}
}
