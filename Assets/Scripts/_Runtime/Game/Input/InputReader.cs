using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Helper;

namespace Personal.InputProcessing
{
	[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader", order = 0)]
	[Serializable]
	public class InputReader : ScriptableObject, PlayerActionInput.IPlayerActions, PlayerActionInput.IUI_OptionActions,
		PlayerActionInput.IUI_InventoryActions, PlayerActionInput.IPuzzleActions
	{
		public event Action<Vector2> OnLookEvent;
		public event Action<Vector2> OnMoveEvent;

		public event Action<bool> OnJumpEvent;

		public event Action<bool> OnSprintEvent;

		public event Action<bool> OnInteractEvent;
		public event Action<bool> OnCancelEvent;

		public event Action<bool> OnInventoryUIOpenEvent;
		public event Action<bool> OnMenuUIOpenEvent;

		public event Action<Vector2> OnDpadEvent;

		public IReadOnlyDictionary<ActionMapType, InputActionMap> InputActionMapDictionary { get => inputActionMapDictionary; }

		Dictionary<ActionMapType, InputActionMap> inputActionMapDictionary = new Dictionary<ActionMapType, InputActionMap>();

		public void Initialize()
		{
			PlayerActionInput playerActionInput = new PlayerActionInput();
			playerActionInput.Player.SetCallbacks(this);
			playerActionInput.UI_Option.SetCallbacks(this);
			playerActionInput.UI_Inventory.SetCallbacks(this);
			playerActionInput.Puzzle.SetCallbacks(this);

			inputActionMapDictionary.Clear();
			inputActionMapDictionary.Add(ActionMapType.Player, playerActionInput.Player);
			inputActionMapDictionary.Add(ActionMapType.UI_Option, playerActionInput.UI_Option);
			inputActionMapDictionary.Add(ActionMapType.UI_Inventory, playerActionInput.UI_Inventory);
			inputActionMapDictionary.Add(ActionMapType.Puzzle, playerActionInput.Puzzle);
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
			OnInteractEvent?.Invoke(context.ReadValue<float>().ConvertToBool());
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			OnCancelEvent?.Invoke(context.ReadValue<float>().ConvertToBool());
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
			OnInventoryUIOpenEvent?.Invoke(context.ReadValue<float>().ConvertToBool());
		}

		void PlayerActionInput.IPlayerActions.OnOptionMenu(InputAction.CallbackContext context)
		{
			OnMenuUIOpenEvent?.Invoke(context.ReadValue<float>().ConvertToBool());
		}

		/// ------------------------------------------------------------
		/// -----------------------PUZZLE-------------------------------
		/// ------------------------------------------------------------

		void PlayerActionInput.IPuzzleActions.OnGamepadSelection(InputAction.CallbackContext context)
		{
			OnDpadEvent?.Invoke(context.ReadValue<Vector2>());
		}
	}
}

