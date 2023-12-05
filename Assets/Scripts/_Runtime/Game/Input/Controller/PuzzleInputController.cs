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
			(ControlInputBase.ActiveControlInput as IPuzzleControlInput)?.Submit();
		}

		void CancelInput()
		{
			IsCancel = true;
			(ControlInputBase.ActiveControlInput as IPuzzleControlInput)?.Cancel();
		}

		void PuzzleReset()
		{
			(ControlInputBase.ActiveControlInput as IPuzzleControlInput)?.Reset();
		}

		void AutoComplete()
		{
			(ControlInputBase.ActiveControlInput as IPuzzleControlInput)?.AutoComplete();
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