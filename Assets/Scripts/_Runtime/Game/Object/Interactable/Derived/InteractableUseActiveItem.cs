using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Item;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractableUseActiveItem : InteractableObject
	{
		[SerializeField] protected ItemType itemTypeCompare = ItemType._10000_Item_1;
		[SerializeField] protected Transform placeAt = null;

		protected override UniTask HandleInteraction()
		{
			HandleUseActiveItem();
			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Check whether it's the correct item type before using it.
		/// </summary>
		void HandleUseActiveItem()
		{
			var activeObject = StageManager.Instance.PlayerController.Inventory.ActiveObject;

			if (activeObject == null) return;
			if (itemTypeCompare != activeObject.ItemType) return;

			Transform fpsTrans = activeObject.PickupableObjectFPS;

			fpsTrans.position = placeAt.position;
			fpsTrans.rotation = Quaternion.identity;
			fpsTrans.localScale = Vector3.one;

			fpsTrans.SetParent(placeAt.parent, true);
			fpsTrans.gameObject.SetActive(true);

			StageManager.Instance.PlayerController.Inventory.UseActiveItem(false);
		}
	}
}

