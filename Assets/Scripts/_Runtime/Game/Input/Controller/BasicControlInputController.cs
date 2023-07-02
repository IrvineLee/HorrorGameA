using UnityEngine;

using Personal.Manager;
using Personal.UI;

namespace Personal.InputProcessing
{
	public class BasicControlInputController : InputControllerBase
	{
		protected override void OnPostEnable()
		{
			inputReaderDefinition.OnMoveEvent += MoveInput;
			inputReaderDefinition.OnLookEvent += LookInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CloseMenu;
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
		}

		void InteractInput()
		{
			IsInteract = true;
		}

		void CancelInput()
		{
			IsCancel = true;
		}

		protected override void OnPostDisable()
		{
			base.OnPostDisable();

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnLookEvent -= LookInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CloseMenu;
		}
	}
}