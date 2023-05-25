using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.UI.Option;

namespace Personal.InputProcessing
{
	public class FPSInputController : InputControllerBase
	{
		public bool IsJump { get; private set; }
		public bool IsSprint { get; private set; }

		protected override async UniTask OnEnable()
		{
			await base.OnEnable();

			inputReaderDefinition.OnMoveEvent += MoveInput;
			inputReaderDefinition.OnLookEvent += LookInput;

			inputReaderDefinition.OnSprintEvent += SprintInput;
			inputReaderDefinition.OnJumpEvent += JumpInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CancelInput;

			inputReaderDefinition.OnMenuUIPressedEvent += OpenOptionMenu;
		}

		void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
		}

		void JumpInput(bool isFlag)
		{
			IsJump = isFlag;
		}

		void SprintInput(bool isFlag)
		{
			IsSprint = isFlag;
		}

		void InteractInput()
		{
			IsInteract = true;
		}

		void CancelInput()
		{
			IsCancel = true;
		}

		void OpenOptionMenu()
		{
			OptionHandlerUI optionHandlerUI = UIManager.Instance.OptionUI;

			InputManager.Instance.EnableActionMap(ActionMapType.UI);
			optionHandlerUI.OpenOptionWindow();
		}

		protected override void OnDisable()
		{
			if (!isAwakeCompleted) return;

			inputReaderDefinition.OnMoveEvent -= MoveInput;
			inputReaderDefinition.OnLookEvent -= LookInput;

			inputReaderDefinition.OnSprintEvent -= SprintInput;
			inputReaderDefinition.OnJumpEvent -= JumpInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CancelInput;

			inputReaderDefinition.OnMenuUIPressedEvent -= OpenOptionMenu;

			base.OnDisable();
		}
	}
}