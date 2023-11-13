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

			if (playerInventory.ActiveObject != null)
			{
				ItemType itemType = playerInventory.ActiveObject.ItemType;
				if (keyItemType == itemType)
				{
					keyItemType = default;
					playerInventory.UseActiveItem();
					return true;
				}
			}

			dialogueSystemTrigger.OnUse(InitiatorStateMachine.transform);
			return false;
		}
	}
}