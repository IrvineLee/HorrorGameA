
using System;

namespace Personal.InteractiveObject
{
	public enum InteractableCompleteType
	{
		NotInteractable = 0,                // Default, Off the collder
		EndDialogue,                        // Still remain interactable but only plays the end dialogue
		RemainInteractable,                 // Still does the interactable
	}
}