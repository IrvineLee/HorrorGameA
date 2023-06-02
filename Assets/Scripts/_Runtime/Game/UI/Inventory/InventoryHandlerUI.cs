using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;
using Personal.System.Handler;
using Helper;
using Personal.Item;

namespace Personal.UI
{
	public class InventoryHandlerUI : MonoBehaviour, IWindowHandler
	{
		[SerializeField] ItemInACircle3DUI itemInACircle3DUI = null;

		public IWindowHandler IWindowHandler { get => this; }

		public event Action<bool> OnMenuOpened;

		List<ItemTypeSet> itemList = new();

		/// <summary>
		/// Add item to canvas camera for ui selection.
		/// </summary>
		/// <param name="itemTypeStr"></param>
		public async void SpawnObject(ItemType itemType)
		{
			GameObject go = await AddressableHelper.Spawn(itemType.GetStringValue(), Vector3.zero, itemInACircle3DUI.transform);
			go.transform.SetLayerAllChildren((int)LayerType._UI);
			itemList.Add(go.GetComponentInChildren<ItemTypeSet>());
		}

		void IWindowHandler.OpenWindow()
		{
			SetWindowEnable(true);
			itemInACircle3DUI.Setup();
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
		}

		void IWindowHandler.CloseWindow()
		{
			SetWindowEnable(false);
			InputManager.Instance.SetToDefaultActionMap();
		}

		void SetWindowEnable(bool isFlag)
		{
			OnMenuOpened?.Invoke(isFlag);
			gameObject.SetActive(isFlag);
		}
	}
}