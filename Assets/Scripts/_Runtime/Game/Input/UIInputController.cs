using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

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

			inputReaderDefinition.OnMoveEvent += MoveInput;

			inputReaderDefinition.OnInteractEvent += InteractInput;
			inputReaderDefinition.OnCancelEvent += CloseOptionMenu;
			inputReaderDefinition.OnMenuUIDefaultPressedEvent += DefaultOptionMenu;
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

		void CloseOptionMenu()
		{
			if (UIManager.Instance.DialogBoxUI.DialogBoxStack.Count == 0)
			{
				UIManager.Instance.OptionUI.CloseOptionWindow();
				return;
			}

			var stack = UIManager.Instance.DialogBoxUI.DialogBoxStack;
			stack.Peek().CancelAction();
		}

		void DefaultOptionMenu()
		{
			UIManager.Instance.OptionUI.ResetToDefault();
		}

		protected override void OnDisable()
		{
			if (!isAwakeCompleted) return;

			inputReaderDefinition.OnMoveEvent -= MoveInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;
			inputReaderDefinition.OnCancelEvent -= CloseOptionMenu;
			inputReaderDefinition.OnMenuUIDefaultPressedEvent -= DefaultOptionMenu;

			base.OnDisable();
		}
	}
}