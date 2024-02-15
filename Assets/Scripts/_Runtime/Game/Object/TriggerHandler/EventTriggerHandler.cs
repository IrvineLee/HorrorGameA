using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Character.Animation;

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

			// Set the state machine.
			InitiatorStateMachine = other.GetComponentInParent<ActorStateMachine>();

			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();
			ifsmHandler?.OnBegin(null);

			var animatorController = InitiatorStateMachine.GetComponentInChildren<AnimatorController>();
			animatorController?.ResetAnimationBlend(0.25f);

			await HandleInteraction();

			ifsmHandler?.OnExit();
			gameObject.SetActive(false);
		}

		protected virtual UniTask<bool> HandleTrigger() { return new UniTask<bool>(true); }
	}
}

