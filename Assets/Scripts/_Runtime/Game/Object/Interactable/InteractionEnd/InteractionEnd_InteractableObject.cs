using System.Collections.Generic;
using UnityEngine;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_InteractableObject : InteractionEnd
	{
		[SerializeField] List<InteractableObject> enableInteractableObjectList = new();
		[SerializeField] List<InteractableObject> disableInteractableObjectList = new();

		protected override void HandleInteractable()
		{
			foreach (var interactable in enableInteractableObjectList)
			{
				interactable?.SetIsInteractable(true);
			}
			foreach (var interactable in disableInteractableObjectList)
			{
				interactable?.SetIsInteractable(false);
			}
		}
	}
}
