using UnityEngine;

using PixelCrushers.DialogueSystem;
using Helper;
using Personal.Item;
using Personal.Save;

namespace Personal.InteractiveObject
{
	public class InteractableDoorLock : InteractableDoor, IDataPersistence
	{
		[SerializeField] ItemType keyItemType = ItemType._10000_Item_1;

		DialogueSystemTrigger dialogueSystemTrigger;

		protected override void Initialize()
		{
			base.Initialize();

			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
		}

		protected override bool IsAbleToOpenDoor()
		{
			if (isInteractionEnded) return true;

			if (playerInventory.ActiveObject != null)
			{
				ItemType itemType = playerInventory.ActiveObject.ItemType;
				if (keyItemType == itemType)
				{
					isInteractionEnded = true;
					playerInventory.UseActiveItem();

					return true;
				}
			}

			dialogueSystemTrigger.OnUse(InitiatorStateMachine.transform);
			return false;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			data.PickupableDictionary.AddOrUpdateValue(id, isInteractionEnded);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.PickupableDictionary.TryGetValue(id, out bool value)) return;

			isInteractionEnded = value;
		}
	}
}