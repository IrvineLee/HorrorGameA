using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.InputProcessing
{
	public class UIInputController : InputControllerBase
	{
		protected override async UniTask Awake()
		{
			await base.Awake();
		}

		protected override async UniTask OnEnable()
		{
			await base.OnEnable();

			inputReader.OnMoveEvent += MoveInput;

			inputReader.OnInteractEvent += InteractInput;
			inputReader.OnCancelEvent += CancelInput;
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

			inputReader.OnMoveEvent -= MoveInput;

			inputReader.OnInteractEvent -= InteractInput;
			inputReader.OnCancelEvent -= CancelInput;

			base.OnDisable();
		}
	}
}