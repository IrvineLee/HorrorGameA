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

			InputManager.Instance.EnableActionMap(ActionMapType.Player);
			inputReader = InputManager.Instance.InputReader;
		}

		protected override async UniTask OnEnable()
		{
			await base.OnEnable();

			inputReader.OnMoveEvent += MoveInput;
			inputReader.OnLookEvent += LookInput;

			inputReader.OnSprintEvent += SprintInput;
			inputReader.OnJumpEvent += JumpInput;

			inputReader.OnInteractEvent += InteractInput;
			inputReader.OnCancelEvent += CancelInput;

			//inputReader.OnInventoryUIPressedEvent += InventoryUIOpenInput;
			//inputReader.OnMenuUIPressedEvent += MenuUIOpenInput;
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

		//void InventoryUIOpenInput()
		//{
		//	IsInventoryUIOpen = true;
		//}

		//void MenuUIOpenInput()
		//{
		//	IsMenuUIOpen = true;
		//}

		void OnDisable()
		{
			if (!isAwakeCompleted) return;

			inputReader.OnMoveEvent -= MoveInput;
			inputReader.OnLookEvent -= LookInput;

			inputReader.OnSprintEvent -= SprintInput;
			inputReader.OnJumpEvent -= JumpInput;

			inputReader.OnInteractEvent -= InteractInput;
			inputReader.OnCancelEvent -= CancelInput;
		}
	}
}