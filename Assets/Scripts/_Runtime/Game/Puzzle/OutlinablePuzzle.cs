using UnityEngine;

using Personal.UI;

namespace Helper
{
	public class OutlinablePuzzle : OutlinableFadeInOut
	{
		GamepadMovement gamepadMovement;

		protected override void Awake()
		{
			base.Awake();
			gamepadMovement = GetComponentInParent<GamepadMovement>(true);
		}

		/// <summary>
		/// Inspector call typically for mouse hover.
		/// </summary>
		public void SetPieceActive()
		{
			gamepadMovement.UpdateCurrentSelection(gameObject);
		}
	}
}
