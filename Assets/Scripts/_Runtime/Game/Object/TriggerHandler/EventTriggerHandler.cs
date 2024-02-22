using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;

namespace Personal.InteractiveObject
{
	/// <summary>
	/// Play an event after triggering.
	/// </summary>
	public class EventTriggerHandler : InteractableEventBegin
	{
		void OnTriggerEnter(Collider other)
		{
			if (!IsInteractable) return;

			HandleTriggerEnter(other).Forget();
		}

		void OnTriggerExit(Collider other)
		{
			HandleTriggerExit(other).Forget();
		}

		protected virtual async UniTask HandleTriggerEnter(Collider other)
		{
			bool isPassed = await HandleTrigger();
			if (!isPassed) return;

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();

			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();
			ifsmHandler?.OnBegin(null);

			await HandleInteraction();

			ifsmHandler?.OnExit();
			gameObject.SetActive(false);
		}

		protected virtual UniTask HandleTriggerExit(Collider other) { return UniTask.CompletedTask; }

		protected virtual UniTask<bool> HandleTrigger() { return new UniTask<bool>(true); }
	}
}

