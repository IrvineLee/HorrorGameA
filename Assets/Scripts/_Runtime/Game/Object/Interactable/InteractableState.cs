
using System;

namespace Personal.InteractiveObject
{
	public enum InteractableState
	{
		Interactable = 0,
		Interactable_BeforeInteractFinished,
		Examinable,
		Requirement,
		EndNonInteractable,
		EndRemainInteractable,
	}
}