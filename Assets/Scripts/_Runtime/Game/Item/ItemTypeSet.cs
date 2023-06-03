
using UnityEngine;

using Personal.GameState;
using Personal.Manager;
using Cysharp.Threading.Tasks;

namespace Personal.Item
{
	public class ItemTypeSet : GameInitialize, IItem
	{
		[SerializeField] ItemType itemType = ItemType.Item_1;

		public ItemType ItemType { get => itemType; }

		public ItemEntity Entity { get; private set; }

		protected async override UniTask Awake()
		{
			await base.Awake();

			Entity = MasterDataManager.Instance.Item.Get(itemType);
		}

		void IItem.Use()
		{
		}

		void IItem.PlaceAt(Vector3 position)
		{
			transform.position = position;
			transform.rotation = Quaternion.identity;
			transform.SetParent(null);
		}
	}
}