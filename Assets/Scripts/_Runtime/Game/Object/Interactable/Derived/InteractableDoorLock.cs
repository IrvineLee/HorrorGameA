using UnityEngine;

using Helper;
using Personal.Item;
using Personal.Save;

namespace Personal.InteractiveObject
{
	public class InteractableDoorLock : InteractableDoor, IDataPersistence
	{
		[SerializeField] ItemType keyItemType = ItemType._10000_Item_1;

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override bool IsAbleToOpenDoor()
		{
			if (interactableState == InteractableState.EndNonInteractable) return true;

			if (playerInventory.ActiveObject != null)
			{
				ItemType itemType = playerInventory.ActiveObject.ItemType;
				if (keyItemType == itemType)
				{
					interactableState = InteractableState.EndNonInteractable;
					playerInventory.UseActiveItem();

					return true;
				}
			}

			dialogueSystemTrigger.OnUse(InitiatorStateMachine.transform);
			return false;
		}

		void IDataPersistence.SaveData(SaveObject data)
		{
			data.SceneObjectSavedData.PickupableDictionary.AddOrUpdateValue(guid, interactableState);
		}

		void IDataPersistence.LoadData(SaveObject data)
		{
			if (!data.SceneObjectSavedData.PickupableDictionary.TryGetValue(guid, out interactableState)) return;
		}
	}
}