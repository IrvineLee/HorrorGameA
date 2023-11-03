using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Item;

namespace Personal.InteractiveObject
{
	public class InteractableDoorLock : InteractableDoor
	{
		[SerializeField] ItemType keyItemType = default;

		InteractableDialogue interactableDialogue;

		protected override void Initialize()
		{
			base.Initialize();

			interactableDialogue = GetComponentInChildren<InteractableDialogue>();
		}

		protected override bool IsAbleToOpenDoor()
		{
			if (keyItemType == default) return true;

			var pickupable = playerInventory.ActiveObject?.PickupableObject;
			if (pickupable && keyItemType == pickupable.ItemType)
			{
				keyItemType = default;
				playerInventory.UseActiveItem(true);
				return true;
			}

			interactableDialogue?.HandleInteraction(InitiatorStateMachine).Forget();
			return false;
		}
	}
}