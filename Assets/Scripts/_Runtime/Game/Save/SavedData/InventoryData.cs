using System;
using System.Collections.Generic;
using UnityEngine;

using Personal.Item;

namespace Personal.Save
{
	[Serializable]
	public class InventoryData
	{
		[SerializeField] List<ItemType> itemList = new();

		public List<ItemType> ItemList { get => itemList; set => itemList = value; }
	}
}