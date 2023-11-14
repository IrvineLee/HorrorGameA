using UnityEngine;
using System.Collections.Generic;

using Personal.Pool;
using Personal.GameState;
using Personal.Character.Player;
using Personal.Item;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.Manager
{
	public class PoolManager : GameInitializeSingleton<PoolManager>
	{
		Dictionary<PoolType, SpawnablePoolBase> spawnablePoolDictionary = new();
		Dictionary<string, GameObject> objectDictionary = new();

		PlayerInventory playerInventory;

		void Start()
		{
			foreach (Transform child in transform)
			{
				var spawnablePool = child.GetComponentInChildren<SpawnablePoolBase>();
				spawnablePoolDictionary.Add(spawnablePool.PoolType, spawnablePool);
			}
		}

		protected override void OnMainScene()
		{
			UnsubscribeEvent();

			playerInventory = StageManager.Instance.PlayerController.Inventory;
			playerInventory.OnUseActiveItem += UseActiveItem;
		}

		public SpawnablePoolBase GetPool(PoolType poolType)
		{
			spawnablePoolDictionary.TryGetValue(poolType, out SpawnablePoolBase spawnablePool);
			return spawnablePool;
		}

		public GameObject GetSpawnedObject(string objectStr)
		{
			objectDictionary.TryGetValue(objectStr, out GameObject parentGO);

			if (parentGO == null || parentGO.transform.childCount <= 0) return null;

			Transform objectTrans = parentGO.transform;

			GameObject go = objectTrans.GetChild(0).gameObject;
			go.transform.SetParent(null);
			go.gameObject.SetActive(true);

			return go;
		}

		public void ReturnSpawnedObject(GameObject go)
		{
			go.SetActive(false);

			if (!objectDictionary.ContainsKey(go.name))
			{
				// Create a new parent under PoolManager.
				GameObject parentGO = new GameObject(go.name);
				parentGO.transform.SetParent(transform);

				go.transform.SetParent(parentGO.transform);

				objectDictionary.Add(go.name, parentGO);
			}
			else if (objectDictionary.TryGetValue(go.name, out GameObject parentGO))
			{
				go.transform.SetParent(parentGO.transform);
			}
		}

		void UseActiveItem(Inventory inventory)
		{
			ReturnSpawnedObject(inventory.PickupableObjectFPS.gameObject);
			ReturnSpawnedObject(inventory.PickupableObjectRotateUI.gameObject);
		}

		void UnsubscribeEvent()
		{
			if (!playerInventory) return;
			playerInventory.OnUseActiveItem -= UseActiveItem;
		}

		void OnDestroy()
		{
			UnsubscribeEvent();
		}
	}
}

