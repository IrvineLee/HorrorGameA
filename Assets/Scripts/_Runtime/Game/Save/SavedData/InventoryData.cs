using UnityEngine;
using System;

using System.Collections.Generic;
using static Personal.Character.Player.PlayerInventory;

namespace Personal.Save
{
	[Serializable]
	public class InventoryData
	{
		[SerializeField] List<Inventory> InventoryList = new();

		public List<Inventory> InventoryList1 { get => InventoryList; set => InventoryList = value; }
	}
}