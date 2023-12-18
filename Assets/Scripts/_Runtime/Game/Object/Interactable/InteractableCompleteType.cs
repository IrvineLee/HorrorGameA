
using System;

namespace Personal.InteractiveObject
{
	public enum InteractableCompleteType
	{
		NotInteractable = 0,                // Default, Off the collder
		EndDialogue,                        // Only play the end dialogue
		RemainInteractable,                 // With a complete state but still remain interactable. Some other gameobject should probably off it when needed.
	}
}