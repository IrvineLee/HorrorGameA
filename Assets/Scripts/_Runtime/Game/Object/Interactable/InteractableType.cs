
using System;

namespace Personal.InteractiveObject
{
	[Flags]
	public enum InteractableType
	{
		ExaminableBeforeKeyEvent = 1 << 0,              // You can only examine the object. Nothing further until you activate it.
														// Ex: Before a key event happen, examinining this object displays dialogue A.

		Requirement = 1 << 1,                           // After key event happened, you must have the required item to proceed further. Shows dialogue B.
		AchieveRequirement_BeforeEvent = 1 << 2,        // After collecting the required item, on interact, after using the item, before animation, show dialogue C1.
		AchieveRequirement_AfterEvent = 1 << 3,         // After collecting the required item, on interact, after using the item/animation ended, show dialogue C2.
		Reward = 1 << 5,                                // After getting the reward, show dialogue D.
		End = 1 << 10,                                  // After getting the reward, ended the interaction, interacting with this object again displays the final dialogue.

		All = Requirement | AchieveRequirement_BeforeEvent | AchieveRequirement_AfterEvent | Reward | End,
	}
}