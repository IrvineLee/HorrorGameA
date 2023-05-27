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
	public class InputReaderDefinition : ScriptableObject, PlayerActionInput.IPlayerActions, PlayerActionInput.IUIActions,
		PlayerActionInput.IPuzzleActions
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
		}

		public event Action<Vector2> OnLookEvent;
		public event Action<Vector2> OnMoveEvent;

		public event Action<bool> OnJumpEvent;

		public event Action<bool> OnSprintEvent;

		public event Action OnInteractEvent;
		public event Action OnCancelEvent;

		public event Action OnInventoryUIPressedEvent;
		public event Action OnMenuUIPressedEvent;
		public event Action OnMenuUIDefaultPressedEvent;

		public event Action<Vector2> OnDpadEvent;

		public IReadOnlyDictionary<ActionMapType, InputControllerInfo> InputActionMapDictionary { get => inputActionMapDictionary; }

		Dictionary<ActionMapType, InputControllerInfo> inputActionMapDictionary = new Dictionary<ActionMapType, InputControllerInfo>();

		public void Initialize()
		{
			PlayerActionInput playerActionInput = new PlayerActionInput();

			playerActionInput.Player.SetCallbacks(this);
			playerActionInput.UI.SetCallbacks(this);
			playerActionInput.Puzzle.SetCallbacks(this);

			inputActionMapDictionary.Clear();
			inputActionMapDictionary.Add(ActionMapType.Player, new InputControllerInfo(playerActionInput.Player, InputManager.Instance.FPSInputController));
			inputActionMapDictionary.Add(ActionMapType.UI, new InputControllerInfo(playerActionInput.UI, InputManager.Instance.UIInputController));
			inputActionMapDictionary.Add(ActionMapType.Puzzle, new InputControllerInfo(playerActionInput.Puzzle, InputManager.Instance.PuzzleInputController));
		}

		/// ------------------------------------------------------------
		/// -----------------------GENERIC------------------------------
		/// ------------------------------------------------------------

		public void OnMove(InputAction.CallbackContext context)
		{
			OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnInteract(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnInteractEvent);
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnCancelEvent);
		}

		/// ------------------------------------------------------------
		/// -----------------------PLAYER-------------------------------
		/// ------------------------------------------------------------

		void PlayerActionInput.IPlayerActions.OnLook(InputAction.CallbackContext context)
		{
			OnLookEvent?.Invoke(context.ReadValue<Vector2>());
		}

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

		/// ------------------------------------------------------------
		/// -----------------------PUZZLE-------------------------------
		/// ------------------------------------------------------------

		void PlayerActionInput.IPuzzleActions.OnGamepadSelection(InputAction.CallbackContext context)
		{
			OnDpadEvent?.Invoke(context.ReadValue<Vector2>());
		}

		/// ------------------------------------------------------------
		/// -----------------------PUZZLE-------------------------------
		/// ------------------------------------------------------------

		void PlayerActionInput.IUIActions.OnDefault(InputAction.CallbackContext context)
		{
			SetButtonEvent(context.started, OnMenuUIDefaultPressedEvent);
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

		void PlayerActionInput.IUIActions.OnNotUsed(InputAction.CallbackContext context) { }
	}
}

