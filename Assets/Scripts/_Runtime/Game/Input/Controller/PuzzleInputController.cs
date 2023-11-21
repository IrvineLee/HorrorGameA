using UnityEngine;

namespace Personal.InputProcessing
{
	public class PuzzleInputController : InputControllerBase
	{
		protected override void OnEnable()
		{
			base.OnEnable();

			inputReaderDefinition.OnMoveEvent += MoveInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CancelInput;

			inputReaderDefinition.OnPuzzleResetEvent += PuzzleReset;
			inputReaderDefinition.OnPuzzleAutoCompleteEvent += AutoComplete;
		}

		void MoveInput(Vector2 direction)
		{
			Move = direction;
		}

		void InteractInput()
		{
			IsInteract = true;
			IControlInput?.ButtonSouth_Submit();
		}

		void CancelInput()
		{
			IsCancel = true;
			IControlInput?.ButtonEast_Cancel();
		}

		void PuzzleReset()
		{
			IControlInput?.ButtonNorth();
		}

		void AutoComplete()
		{
			IControlInput?.R3();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CancelInput;

			inputReaderDefinition.OnPuzzleResetEvent -= PuzzleReset;
			inputReaderDefinition.OnPuzzleAutoCompleteEvent -= AutoComplete;
		}
	}
}