
using UnityEngine;

namespace Personal.Item
{
	public class ItemTypeSet : MonoBehaviour
	{
		[SerializeField] ItemType itemType = ItemType.Item_1;

		public ItemType ItemType { get => itemType; }
	}
}