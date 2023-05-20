using UnityEngine;
using UnityEngine.InputSystem;

using Personal.Manager;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.InputProcessing
{
	public class FPSInputController : GameInitialize
	{
		public Vector2 Move { get; private set; }
		public Vector2 Look { get; private set; }

		public bool IsJump { get; private set; }
		public bool IsSprint { get; private set; }
		public bool IsInteract { get; private set; }
		public bool IsCancel { get; private set; }
		public bool IsInventoryUIOpen { get; private set; }
		public bool IsMenuUIOpen { get; private set; }

		InputReader inputReader;

		protected override async UniTask Awake()
		{
			await base.Awake();

			PlayerActionInput playerActionInput = InputManager.Instance.PlayerActionInput;
			InputManager.Instance.EnableActionMap(ActionMapType.Player);

			inputReader = InputManager.Instance.InputReader;

			inputReader.OnMoveEvent += MoveInput;
			inputReader.OnLookEvent += LookInput;

			inputReader.OnSprintEvent += SprintInput;
			inputReader.OnJumpEvent += JumpInput;

			inputReader.OnInteractEvent += InteractInput;
			inputReader.OnCancelEvent += CancelInput;

			inputReader.OnInventoryUIOpenEvent += InventoryUIOpenInput;
			inputReader.OnMenuUIOpenEvent += MenuUIOpenInput;
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

		void InteractInput(bool isFlag)
		{
			IsInteract = isFlag;
			Debug.Log(IsInteract);
		}

		void CancelInput(bool isFlag)
		{
			IsCancel = isFlag;
		}

		void InventoryUIOpenInput(bool isFlag)
		{
			IsInventoryUIOpen = isFlag;
		}

		void MenuUIOpenInput(bool isFlag)
		{
			IsMenuUIOpen = isFlag;
		}

		void OnDestroy()
		{
			inputReader.OnMoveEvent -= MoveInput;
			inputReader.OnLookEvent -= LookInput;

			inputReader.OnSprintEvent -= SprintInput;
			inputReader.OnJumpEvent -= JumpInput;

			inputReader.OnInteractEvent -= InteractInput;
			inputReader.OnCancelEvent -= CancelInput;
		}
	}
}