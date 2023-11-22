using UnityEngine;

using Personal.Puzzle;
using Personal.Manager;
using Personal.InputProcessing;

namespace Helper
{
	public class OutlinablePuzzle : OutlinableFadeInOut
	{
		PuzzleController puzzleController;

		protected override void Awake()
		{
			base.Awake();

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
			((ControlInput)ControlInputBase.ActiveControlInput).UpdateCurrentSelection(gameObject);
		}
	}
}
