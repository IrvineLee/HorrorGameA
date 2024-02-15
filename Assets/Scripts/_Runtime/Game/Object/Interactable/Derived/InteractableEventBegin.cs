using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.InteractiveObject
{
	public class InteractableEventBegin : InteractableObject
	{
		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;
		protected PlayerController playerController;

		protected override void Initialize()
		{
			base.Initialize();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
			playerController = StageManager.Instance.PlayerController;
		}

		protected override async UniTask HandleInteraction()
		{
			// When events happened, hide the item and pause the FSM.
			playerController.Inventory.FPS_HideItem();
			playerController.PauseControl(true);

			InputManager.Instance.DisableAllActionMap();

			await orderedStateMachine.Begin(interactionAssign, InitiatorStateMachine);
			await base.HandleInteraction();

			// Reset to default.
			InputManager.Instance.SetToDefaultActionMap();
			playerController.PauseControl(false);
		}

		protected override bool IsCompleteInteraction()
		{
			return interactionAssign.IsComplete;
		}
	}
}

