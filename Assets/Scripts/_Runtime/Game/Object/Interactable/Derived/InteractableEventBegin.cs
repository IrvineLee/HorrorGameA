using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.InteractiveObject
{
	public class InteractableEventBegin : InteractableObject
	{
		protected InteractionAssign interactionAssign;
		protected PlayerController playerController;

		protected override void Initialize()
		{
			base.Initialize();

			interactionAssign = GetComponentInChildren<InteractionAssign>();
			playerController = StageManager.Instance.PlayerController;
		}

		protected override async UniTask HandleInteraction()
		{
			// When events happened, hide the item and pause the FSM.
			playerController.Inventory.FPS_HideItem();
			playerController.PauseControl(true);

			InputManager.Instance.DisableAllActionMap();

			if (interactionAssign) await interactionAssign.BeginFSM(InitiatorStateMachine);
			await base.HandleInteraction();

			// Reset to default.
			InputManager.Instance.SetToDefaultActionMap();
			playerController.PauseControl(false);
		}

		protected override bool IsCompleteInteraction()
		{
			return interactionAssign ? interactionAssign.IsProcessComplete : true;
		}
	}
}

