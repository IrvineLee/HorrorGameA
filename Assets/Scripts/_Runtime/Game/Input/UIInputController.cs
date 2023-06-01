using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.UI;

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
			inputReaderDefinition.OnCancelEvent += CloseInventoryMenu;

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
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Option) return;

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

		void CloseInventoryMenu()
		{
			if (UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Inventory) return;
		}

		protected override void OnDisable()
		{
			if (!isAwakeCompleted) return;

			inputReaderDefinition.OnMoveEvent -= MoveInput;

			inputReaderDefinition.OnInteractEvent -= InteractInput;

			inputReaderDefinition.OnCancelEvent -= CloseOptionMenu;
			inputReaderDefinition.OnCancelEvent -= CloseInventoryMenu;

			inputReaderDefinition.OnMenuUIDefaultPressedEvent -= DefaultOptionMenu;

			base.OnDisable();
		}
	}
}