using UnityEngine;

using Personal.FSM;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler : InteractableObjectEventStateChange
	{
		async void OnTriggerEnter(Collider other)
		{
			// Disable the trigger collider.
			currentCollider.enabled = false;

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();
			await base.HandleInteraction();
		}
	}
}

