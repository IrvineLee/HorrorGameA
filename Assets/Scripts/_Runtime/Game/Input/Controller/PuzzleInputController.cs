using UnityEngine;

using Helper;
using Personal.Puzzle;

namespace Personal.InputProcessing
{
	public class PuzzleInputController : InputControllerBase
	{
		IControlInput iControlInput;

		void OnEnable()
		{
			inputReaderDefinition.OnMoveEvent += MoveInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CancelInput;

			inputReaderDefinition.OnPuzzleResetEvent += PuzzleReset;
			inputReaderDefinition.OnPuzzleAutoCompleteEvent += AutoComplete;

			// PuzzleController will always sets it's ActiveController first before activating this input controller.
			iControlInput = PuzzleController.ActiveController?.GetComponent<IControlInput>();
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void InteractInput()
		{
			IsInteract = true;
			iControlInput?.Submit();
		}

		void CancelInput()
		{
			IsCancel = true;
			iControlInput?.Cancel();
		}

		void PuzzleReset()
		{
			iControlInput?.ButtonNorth();
		}

		void AutoComplete()
		{
			iControlInput?.R3();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CancelInput;

			inputReaderDefinition.OnPuzzleResetEvent -= PuzzleReset;
			inputReaderDefinition.OnPuzzleAutoCompleteEvent -= AutoComplete;

			iControlInput = null;
		}
	}
}