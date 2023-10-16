using UnityEngine;

using Personal.UI;
using Personal.Puzzle;
using Personal.Manager;

namespace Helper
{
	public class OutlinablePuzzle : OutlinableFadeInOut
	{
		GamepadMovement gamepadMovement;
		PuzzleController puzzleController;

		protected override void Awake()
		{
			base.Awake();

			gamepadMovement = GetComponentInParent<GamepadMovement>(true);
			puzzleController = GetComponentInParent<PuzzleController>(true);
		}

		/// <summary>
		/// Inspector call typically for mouse hover.
		/// </summary>
		public void SetPieceActive()
		{
			if (puzzleController == null) return;
			if (!puzzleController.enabled) return;

			if (!InputManager.Instance.IsCurrentDeviceMouse) return;
			gamepadMovement.UpdateCurrentSelection(gameObject);
		}
	}
}
