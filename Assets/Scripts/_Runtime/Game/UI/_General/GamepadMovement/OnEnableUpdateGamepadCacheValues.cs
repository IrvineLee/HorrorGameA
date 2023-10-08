using UnityEngine;

using Personal.GameState;
using UnityEngine.UI;

namespace Personal.UI
{
	/// <summary>
	/// This is typically used when UIs are instantiated in game, and the gamepad needs to recache it's values(buttons/sliders/etc).
	/// </summary>
	public class OnEnableUpdateGamepadCacheValues : GameInitialize
	{
		UIGamepadMovement uiGamepadMovement;
		ScrollRect scrollRect;

		protected override void Initialize()
		{
			uiGamepadMovement = GetComponentInParent<UIGamepadMovement>(true);
		}

		void OnEnable()
		{
			if (!uiGamepadMovement) return;

			uiGamepadMovement.RefreshCacheValues();
			uiGamepadMovement.AutoScrollRect.SetSelectionToTop();
		}
	}
}
