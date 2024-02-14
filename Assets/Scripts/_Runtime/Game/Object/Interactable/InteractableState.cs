
namespace Personal.InteractiveObject
{
	public enum InteractableState
	{
		Interactable = 0,                   // It is still interactable.
		Interactable_OnlyOnceFinished,      // It has finished the only once animation trigger.
		Examinable,                         // Examinable state.
		Requirement,                        // Requirement state. This just checks your inventory items with the required list.
		CompleteInteraction,                // Main interaction has been completed.

		EndNonInteractable = 100,           // Interaction ended. No more further interaction.
		EndDialogue,                        // Interaction ended. Only trigger end dialogue.
		EndRemainInteractable,              // Interaction ended. Still can trigger end interaction.
	}
}