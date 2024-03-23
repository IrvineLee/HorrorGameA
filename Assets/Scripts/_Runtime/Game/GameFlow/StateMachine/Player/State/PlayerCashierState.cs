using UnityEngine;

using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerCashierState : PlayerStandardState
	{
		protected override void HandleOnInteractable(RaycastHit hit)
		{
			if (!InputManager.Instance.FPSInputController.IsInteract) return;

			hit.transform.gameObject.SetActive(false);
		}
	}
}