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
			((IPuzzleControlInput)controlInput)?.Submit();
		}

		void CancelInput()
		{
			IsCancel = true;
			((IPuzzleControlInput)controlInput)?.Cancel();
		}

		void PuzzleReset()
		{
			((IPuzzleControlInput)controlInput)?.Reset();
		}

		void AutoComplete()
		{
			((IPuzzleControlInput)controlInput)?.AutoComplete();
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