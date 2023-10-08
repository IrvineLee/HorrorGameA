using UnityEngine;

using Personal.FSM;

namespace Personal.InteractiveObject
{
	public class EventTriggerHandler : InteractableObjectEventStateChange
	{
		bool isTriggerable;

		public void SetIsTriggerable(bool isFlag) { isTriggerable = isFlag; }

		async void OnTriggerEnter(Collider other)
		{
			if (!isTriggerable) return;

			// Disable the trigger collider.
			currentCollider.enabled = false;

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();
			await base.HandleInteraction();
		}
	}
}

