using UnityEngine;
using System.Collections.Generic;

using Helper;
using Personal.Pool;
using Personal.GameState;

namespace Personal.Manager
{
	public class PoolManager : GameInitializeSingleton<PoolManager>
	{
		Dictionary<PoolType, SpawnablePoolBase> spawnablePoolDictionary = new();
		Dictionary<string, GameObject> actorDictionary = new();

		void Start()
		{
			foreach (Transform child in transform)
			{
				var spawnablePool = child.GetComponentInChildren<SpawnablePoolBase>();
				spawnablePoolDictionary.Add(spawnablePool.PoolType, spawnablePool);
			}
		}

		public SpawnablePoolBase GetPool(PoolType poolType)
		{
			spawnablePoolDictionary.TryGetValue(poolType, out SpawnablePoolBase spawnablePool);
			return spawnablePool;
		}

		public GameObject GetSpawnedActor(string actorStr)
		{
			actorDictionary.TryGetValue(actorStr, out GameObject go);
			go?.transform.SetParent(null);
			go?.gameObject.SetActive(true);

			return go;
		}

		public void ReturnSpawnedActor(GameObject go)
		{
			go.transform.SetParent(transform);
			go.gameObject.SetActive(false);

			if (!actorDictionary.ContainsKey(go.name))
			{
				actorDictionary.Add(go.name, go);
			}
		}
	}
}

