using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.InputProcessing
{
	public class PuzzleInputController : InputControllerBase
	{
		protected override async UniTask OnEnable()
		{
			await base.OnEnable();

			inputReaderDefinition.OnMoveEvent += MoveInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CancelInput;
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void InteractInput()
		{
			IsInteract = true;
		}

		void CancelInput()
		{
			IsCancel = true;
		}

		protected override void OnDisable()
		{
			if (!isAwakeCompleted) return;

			inputReaderDefinition.OnMoveEvent -= MoveInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CancelInput;

			base.OnDisable();
		}
	}
}