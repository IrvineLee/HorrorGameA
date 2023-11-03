using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.Item;

namespace Personal.InteractiveObject
{
	public class InteractableDoorLock : InteractableDoor
	{
		[SerializeField] ItemType keyItemType = default;

		DialogueSystemTrigger dialogueSystemTrigger;

		protected override void Initialize()
		{
			base.Initialize();

			dialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
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

			dialogueSystemTrigger.OnUse(InitiatorStateMachine.transform);
			return false;
		}
	}
}