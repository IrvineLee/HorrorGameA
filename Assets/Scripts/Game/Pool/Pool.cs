using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Personal.Pool
{
	public class Pool : MonoBehaviour
	{
		[SerializeField] AssetReference assetReference = null;
		[SerializeField] int defaultCapacity = 10;
		[SerializeField] int maxPoolSize = 50;

		ObjectPool<GameObject> pool;

		void Start()
		{
			Initialize();
		}

		public GameObject Get()
		{
			return pool.Get();
		}

		async void Initialize()
		{
			var pooledItem = await CreatePooledItem();
			if (!pooledItem) return;

			pool = new ObjectPool<GameObject>(() => pooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
		}

		async UniTask<GameObject> CreatePooledItem()
		{
			return await assetReference.InstantiateAsync(transform);
		}

		void OnTakeFromPool(GameObject pooledObj)
		{
			pooledObj.gameObject.SetActive(true);
		}

		void OnReturnedToPool(GameObject pooledObj)
		{
			pooledObj.gameObject.SetActive(false);
		}

		void OnDestroyPoolObject(GameObject pooledObj)
		{
			Destroy(pooledObj.gameObject);
		}
	}

}
