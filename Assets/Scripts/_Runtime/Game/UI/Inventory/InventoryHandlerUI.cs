using System;
using UnityEngine;

using Personal.InputProcessing;
using Personal.Manager;

namespace Personal.UI
{
	public class InventoryHandlerUI : MonoBehaviour, IWindowHandler
	{
		public event Action<bool> OnMenuOpened;

		void IWindowHandler.OpenWindow()
		{
			SetWindowEnable(true);
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