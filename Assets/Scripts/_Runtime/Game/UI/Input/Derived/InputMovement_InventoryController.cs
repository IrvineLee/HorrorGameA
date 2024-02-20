using System;
using UnityEngine;

using Personal.UI;

namespace Personal.InputProcessing
{
	public class InputMovement_InventoryController : ControlInput, IUIControlInput
	{
		protected ItemInACircle3DUI itemInACircle3DUI;

		protected override void Initialize()
		{
			itemInACircle3DUI = GetComponentInChildren<ItemInACircle3DUI>(true);
		}

		protected override void HandleMovement(Vector2 move, Action endConfirmButtonAction = default)
		{
			itemInACircle3DUI.HandleInput(move.x < 0 || move.y < 0);
		}
	}
}