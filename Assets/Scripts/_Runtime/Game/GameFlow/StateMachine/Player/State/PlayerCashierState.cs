using UnityEngine;

using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerCashierState : PlayerBaseState
	{
		protected override void HandleInteractable(RaycastHit hit)
		{
			if (!InputManager.Instance.FPSInputController.IsInteract) return;

			hit.transform.gameObject.SetActive(false);
		}
	}
}