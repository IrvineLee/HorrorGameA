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

			inputReader.OnMoveEvent += MoveInput;
			inputReader.OnLookEvent += LookInput;

			inputReader.OnSprintEvent += SprintInput;
			inputReader.OnJumpEvent += JumpInput;

			inputReader.OnInteractEvent += InteractInput;
			inputReader.OnCancelEvent += CancelInput;

			inputReader.OnMenuUIPressedEvent += OpenOptionMenu;
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
			if (optionHandlerUI.IsOpened) return;

			InputManager.Instance.EnableActionMap(ActionMapType.UI);
			optionHandlerUI.OpenMenuTab(OptionHandlerUI.MenuTab.Graphic);
		}

		protected override void OnDisable()
		{
			if (!isAwakeCompleted) return;

			inputReader.OnMoveEvent -= MoveInput;
			inputReader.OnLookEvent -= LookInput;

			inputReader.OnSprintEvent -= SprintInput;
			inputReader.OnJumpEvent -= JumpInput;

			inputReader.OnInteractEvent -= InteractInput;
			inputReader.OnCancelEvent -= CancelInput;

			inputReader.OnMenuUIPressedEvent -= OpenOptionMenu;

			base.OnDisable();
		}
	}
}