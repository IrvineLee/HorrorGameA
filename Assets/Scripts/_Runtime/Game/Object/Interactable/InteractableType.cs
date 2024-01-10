
using System;

namespace Personal.InteractiveObject
{
	[Flags]
	public enum InteractableType
	{
		ExaminableBeforeKeyEvent = 1 << 0,              // You can only examine the object. Nothing further until you activate it.
														// Ex: Before a key event happen, examinining this object displays dialogue A.

		Requirement = 1 << 1,                           // After key event happened, you must have the required item to proceed further. Shows dialogue B.
		AchieveRequirement_BeforeInteract = 1 << 2,     // After collecting the required item, before interact, before animation, show dialogue C1.
		AchieveRequirement_AfterInteract = 1 << 3,      // After collecting the required item, after interact, after animation ended, show dialogue C2.
		Reward = 1 << 5,                                // After getting the reward, show dialogue D.
		EndRemainInteractable = 1 << 10,                // After getting the reward, ended the interaction, interacting with this object again displays the end dialogue.

		All = ExaminableBeforeKeyEvent | Requirement | AchieveRequirement_BeforeInteract | AchieveRequirement_AfterInteract | Reward | EndRemainInteractable,
	}
}