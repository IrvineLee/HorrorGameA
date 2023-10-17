
using UnityEngine;

using Personal.GameState;
using Personal.Manager;

namespace Personal.Item
{
	public class ItemTypeSet : GameInitialize, IItem
	{
		[SerializeField] ItemType itemType = ItemType._10000_Item_1;

		public ItemType ItemType { get => itemType; }

		public ItemEntity Entity { get; private set; }

		protected override void Initialize()
		{
			if (itemType == default) return;
			Entity = MasterDataManager.Instance.Item.Get((int)itemType);
		}

		void IItem.Use()
		{
		}

		void IItem.PlaceAt(Vector3 position, Transform parent)
		{
			transform.position = position;
			transform.rotation = Quaternion.identity;
			transform.SetParent(parent);
		}
	}
}