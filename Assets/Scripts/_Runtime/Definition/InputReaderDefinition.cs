using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Helper;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Input/InputReaderDefinition", order = 0)]
	[Serializable]
	public class InputReaderDefinition : ScriptableObject, PlayerActionInput.IPlayerActions, PlayerActionInput.IUIActions, PlayerActionInput.IPuzzleActions
	{
		[Serializable]
		public class InputControllerInfo
		{
			public InputActionMap InputActionMap { get; private set; }
			public InputControllerBase InputController { get; private set; }

			public InputControllerInfo(InputActionMap inputActionMap, InputControllerBase inputController)
			{
				InputActionMap = inputActionMap;
				InputController = inputController;
			}

			public void Enable(bool isFlag)
			{
				if (isFlag) InputActionMap.Enable();
				else InputActionMap.Disable();

				InputController.enabled = isFlag;
			}
		}

		// FPS controls
		public event Action<Vector2> OnLookEvent;
		public event Action<Vector2> OnMoveEvent;

		public event Action<bool> OnJumpEvent;
		public event Action<bool> OnSprintEvent;

		public event Action OnInteractEvent;
		public event Action OnCancelEvent;

		// UI/Options
		public event Action OnInventoryUIPressedEvent;
		public event Action OnMenuUIPressedEvent;
		public event Action OnMenuUIDefaultPressedEvent;
		public event Action<bool> OnTabSwitchEvent;

		public event Action OnMenuUICancelledEvent;

		// Inventory
		public event Action<int> OnInventoryNextPreviousEvent;
		public event Action<int> OnInventoryIndexSelectEvent;
		public PlayerActionInput PlayerActionInput { get; private set; }
		public IReadOnlyDictionary<ActionMapType, InputControllerInfo> InputActionMapDictionary { get => inputActionMapDictionary; }

		Dictionary<ActionMapType, InputControllerInfo> inputActionMapDictionary = new Dictionary<ActionMapType, InputControllerInfo>();
		bool isUSInteract = true;

		public void Initialize()
		{
			PlayerActionInput = new PlayerActionInput();

			PlayerActionInput.Player.SetCallbacks(this);
			PlayerActionInput.UI.SetCallbacks(this);
			PlayerActionInput.Puzzle.SetCallbacks(this);

			inputActionMapDictionary.Clear();
			inputActionMapDictionary.Add(ActionMapType.Player, new InputControllerInfo(PlayerActionInput.Player, InputManager.Instance.FPSInputController));
			inputActionMapDictionary.Add(ActionMapType.UI, new InputControllerInfo(PlayerActionInput.UI, InputManager.Instance.UIInputController));
			inputActionMapDictionary.Add(ActionMapType.Puzzle, new InputControllerInfo(PlayerActionInput.Puzzle, InputManager.Instance.PuzzleInputController));

			foreach (var actionMap in inputActionMapDictionary)
			{
				actionMap.Value.InputController.Initialize();
			}
		}

		public void SwapInteractInput(bool isUSInteract)
		{
			this.isUSInteract = isUSInteract;
		}

		/// ------------------------------------------------------------
		/// -----------------------GENERIC------------------------------
		/// ------------------------------------------------------------

		public void OnMove(InputAction.CallbackContext context)
		{
			OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			OnLookEvent?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnInteract(InputAction.CallbackContext context)
		{
			bool isMouse = InputManager.Instance.IsCurrentDeviceMouse;

			if (isMouse || (!isMouse && isUSInteract))
			{
				SetButtonEvent(context.started, OnInteractEvent);
				return;
			}
			SetButtonEvent(context.started, OnCancelEvent);
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			bool isMouse = InputManager.Instance.IsCurrentDeviceMouse;

			if (isMouse || (!isMouse && isUSInteract))
			{
				SetButtonEvent(context.started, OnCancelEvent);
				return;
			}
			SetButtonEvent(context.started, OnInteractEvent);
		}

		/// ------------------------------------------------------------
		/// -----------------------PLAYER-------------------------------
		/// ------------------------------------------------------------

		void PlayerActionInput.IPlayerActions.OnJump(InputAction.CallbackContext context)
		{
			OnJumpEvent?.Invoke(context.ReadValue<float>().ConvertToBool());
		}

		void PlayerActionInput.IPlayerActions.OnSprint(InputAction.CallbackContext context)
		{
			OnSprintEvent?.Invoke(context.ReadValue<float>().ConvertToBool());
		}

		void PlayerActionInput.IPlayerActions.OnInventoryMenu(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnInventoryUIPressedEvent);
		}

		void PlayerActionInput.IPlayerActions.OnOptionMenu(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnMenuUIPressedEvent);
		}

		void PlayerActionInput.IPlayerActions.OnInventoryMouseScroll(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, () => OnInventoryNextPreviousEvent?.Invoke((int)context.ReadValue<float>()));
		}

		void PlayerActionInput.IPlayerActions.OnInventoryNextPrevious(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, () => OnInventoryNextPreviousEvent?.Invoke((int)context.ReadValue<float>()));
		}

		void PlayerActionInput.IPlayerActions.OnInventoryIndexSelect(InputAction.CallbackContext context)
		{
			OnInventoryIndexSelectEvent?.Invoke((int)context.ReadValue<float>());
		}

		/// ------------------------------------------------------------
		/// -----------------------UI-------------------------------
		/// ------------------------------------------------------------

		void PlayerActionInput.IUIActions.OnDefault(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnMenuUIDefaultPressedEvent);
		}

		void PlayerActionInput.IUIActions.OnTabSwitch(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, () => OnTabSwitchEvent?.Invoke(context.ReadValue<float>().ConvertToBool()));
		}

		void PlayerActionInput.IUIActions.OnCancelOptionMenu(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnMenuUICancelledEvent);
		}

		/// ------------------------------------------------------------
		/// -----------------------OTHERS-------------------------------
		/// ------------------------------------------------------------

		void SetButtonEvent(bool isFlag, Action doEvent)
		{
			if (!isFlag) return;
			doEvent?.Invoke();
		}

		/// ------------------------------------------------------------
		/// ----------------------NOT USED------------------------------
		/// ------------------------------------------------------------

		// To get triggers for analog movement to update UI icons.
		void PlayerActionInput.IUIActions.OnNotUsed(InputAction.CallbackContext context) { }

		// For the Input System UI Input Module.
		void PlayerActionInput.IUIActions.OnPoint(InputAction.CallbackContext context) { }
		void PlayerActionInput.IUIActions.OnClick(InputAction.CallbackContext context) { }
		void PlayerActionInput.IUIActions.OnScrollWheel(InputAction.CallbackContext context) { }
		void PlayerActionInput.IUIActions.OnMiddleClick(InputAction.CallbackContext context) { }
		void PlayerActionInput.IUIActions.OnRightClick(InputAction.CallbackContext context) { }
		void PlayerActionInput.IUIActions.OnTrackedDevicePosition(InputAction.CallbackContext context) { }
		void PlayerActionInput.IUIActions.OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
	}
}

