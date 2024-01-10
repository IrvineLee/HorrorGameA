using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;

namespace Personal.InteractiveObject
{
	/// <summary>
	/// All event trigger will end after triggering them.
	/// </summary>
	public class EventTriggerHandler : InteractableEventBegin
	{
		async void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;

			bool isPassed = await HandleTrigger();

			if (!isPassed) return;

			interactableState = InteractableState.EndNonInteractable;
			gameObject.SetActive(false);

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();
			base.HandleInteraction().Forget();
		}

		protected virtual UniTask<bool> HandleTrigger() { return new UniTask<bool>(true); }
	}
}

