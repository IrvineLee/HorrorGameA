using UnityEngine;
using System;

using Helper;
using Personal.Item;

namespace Personal.Save
{
	[Serializable]
	public class GlossaryData
	{
		[SerializeField] SerializableDictionary<ItemType, int> usedItemDictionary = new();

		public SerializableDictionary<ItemType, int> UsedItemDictionary { get => usedItemDictionary; set => usedItemDictionary = value; }
	}
}