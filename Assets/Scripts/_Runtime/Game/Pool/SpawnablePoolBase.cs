using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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
		[SerializeField] string key = "";

		[Space]
		[SerializeField] int initialSpawn = 5;
		[SerializeField] int defaultCapacity = 10;
		[SerializeField] int maxPoolSize = 50;

		public PoolType PoolType { get => poolType; }

		ObjectPool<GameObject> pool;

		void Start()
		{
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
			go.transform.SetParent(transform);
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
			InitalSpawn();
		}

		async UniTask<bool> LoadAddressable()
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
			await UniTask.WaitUntil(() => handle.Status != AsyncOperationStatus.None);

			return handle.Status == AsyncOperationStatus.Succeeded;
		}

		void InitalSpawn()
		{
			List<GameObject> goList = new();
			for (int i = 0; i < initialSpawn; i++)
			{
				goList.Add(Get());
			}
			for (int i = 0; i < goList.Count; i++)
			{
				goList[i].GetComponentInChildren<SpawnableObject>()?.SetNotToReturnOnDisable();
				Return(goList[i]);
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

		protected virtual void OnReturnedToPool(GameObject pooledObj)
		{
			pooledObj.gameObject.SetActive(false);
		}

		protected virtual void OnDestroyPoolObject(GameObject pooledObj)
		{
			Destroy(pooledObj.gameObject);
		}

		void OnValidate()
		{
			transform.name = "Pool_" + key;
		}
	}

}
