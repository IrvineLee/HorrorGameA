
using System;

namespace Personal.InteractiveObject
{
	public enum InteractableState
	{
		Interactable = 0,
		InteractableBeforeInteractFinished,
		Examinable,
		Requirement,
		EndNonInteractable,
		EndRemainInteractable,
	}
}