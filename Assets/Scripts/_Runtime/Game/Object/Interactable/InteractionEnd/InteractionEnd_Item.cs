using System.Collections.Generic;
using UnityEngine;

using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_Item : InteractionEnd
	{
		[SerializeField] List<GameObject> goList = new();

		List<InteractablePickupable> interactablePickupableList = new();

		protected override void Initialize()
		{
			foreach (var go in goList)
			{
				var interactable = go.GetComponentInChildren<InteractablePickupable>();

				if (!interactable) continue;
				interactablePickupableList.Add(interactable);
			}
		}

		protected override void HandleInteractable()
		{
			base.HandleInteractable();

			foreach (var interactable in interactablePickupableList)
			{
				StageManager.Instance.PlayerController.Inventory.AddItem(interactable);
			}
		}
	}
}

