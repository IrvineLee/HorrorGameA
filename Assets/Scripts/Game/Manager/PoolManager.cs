using UnityEngine;
using System.Collections.Generic;

using Helper;
using Personal.Pool;

namespace Personal.Manager
{
	public class PoolManager : MonoBehaviourSingleton<PoolManager>
	{
		Dictionary<PoolType, SpawnablePoolBase> spawnablePoolDictionary = new();

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
	}
}

