using System;
using UnityEngine;

namespace Personal.Item
{
	[Serializable]
	public class ItemData
	{
		[SerializeField] ItemType itemType = ItemType._10100_PuzzleBlock_A;
		[SerializeField] int amount = 1;

		public ItemType ItemType { get => itemType; }
		public int Amount { get => amount; }
	}
}