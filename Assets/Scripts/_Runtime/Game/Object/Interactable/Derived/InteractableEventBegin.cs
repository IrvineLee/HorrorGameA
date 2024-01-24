using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class InteractableEventBegin : InteractableObject
	{
		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected override void Initialize()
		{
			base.Initialize();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		protected override async UniTask HandleInteraction()
		{
			// When events happened, hide the items.
			StageManager.Instance.PlayerController.Inventory.FPS_HideItem();

			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();
			ifsmHandler?.OnBegin(null);

			await orderedStateMachine.Begin(interactionAssign, InitiatorStateMachine);

			InputManager.Instance.SetToDefaultActionMap();
			ifsmHandler?.OnExit();
		}
	}
}

