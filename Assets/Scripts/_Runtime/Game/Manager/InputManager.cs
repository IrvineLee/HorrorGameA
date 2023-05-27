using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.Definition;
using Helper;
using Personal.Setting.Game;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
		[SerializeField] InputReaderDefinition inputReaderDefinition = null;
		[SerializeField] ActionMapType defaultActionMap = ActionMapType.Player;
		[SerializeField] ButtonIconDefinition buttonIconDefinition = null;

		public InputReaderDefinition InputReaderDefinition { get => inputReaderDefinition; }
		public PlayerActionInput PlayerActionInput { get; private set; }
		public ButtonIconDefinition ButtonIconDefinition { get => buttonIconDefinition; }

		// Different actions maps for different situations.
		public FPSInputController FPSInputController { get; private set; }
		public UIInputController UIInputController { get; private set; }
		public PuzzleInputController PuzzleInputController { get; private set; }

		public ActionMapType CurrentActionMapType { get; private set; }
		public InputDeviceType InputDeviceType { get; private set; } = InputDeviceType.None;

		public bool IsInteract
		{
			get
			{
				switch (CurrentActionMapType)
				{
					case ActionMapType.UI: return UIInputController.IsInteract;
					case ActionMapType.Puzzle: return PuzzleInputController.IsInteract;
					default: return FPSInputController.IsInteract;
				}
			}
		}

		public bool IsCancel
		{
			get
			{
				switch (CurrentActionMapType)
				{
					case ActionMapType.UI: return UIInputController.IsCancel;
					case ActionMapType.Puzzle: return PuzzleInputController.IsCancel;
					default: return FPSInputController.IsCancel;
				}
			}
		}

		public string IconInitials { get; private set; }

		public event Action OnDeviceIconChanged;

		InputDevice previousDevice = null;

		int gamepadIconIndex;

		protected override async UniTask Awake()
		{
			await base.Awake();

			FPSInputController = GetComponentInChildren<FPSInputController>();
			UIInputController = GetComponentInChildren<UIInputController>();
			PuzzleInputController = GetComponentInChildren<PuzzleInputController>();

			inputReaderDefinition.Initialize();
			SetToDefaultActionMap();

			InputSystem.onActionChange += HandleInputDeviceType;
			InputSystem.onAnyButtonPress.Call(ctrl => HandleInputDeviceCompare(ctrl.device.displayName));
		}

		/// <summary>
		/// Enable action map type.
		/// </summary>
		/// <param name="actionMap"></param>
		public void EnableActionMap(ActionMapType actionMap)
		{
			// Disable all action map.
			foreach (var map in inputReaderDefinition.InputActionMapDictionary)
			{
				map.Value.InputActionMap.Disable();
				map.Value.InputController.enabled = false;
			}

			// Enable specified actin map.
			inputReaderDefinition.InputActionMapDictionary.TryGetValue(actionMap, out var inputActionMap);
			inputActionMap.InputActionMap.Enable();
			inputActionMap.InputController.enabled = true;

			CurrentActionMapType = actionMap;
		}

		/// <summary>
		/// Reset back to default action map.
		/// </summary>
		public void SetToDefaultActionMap()
		{
			EnableActionMap(defaultActionMap);
		}

		/// <summary>
		/// Set gamepad icon index which will change the UI display.
		/// </summary>
		/// <param name="gamepadIconIndex"></param>
		public void SetGamepadIconIndex(int gamepadIconIndex)
		{
			this.gamepadIconIndex = gamepadIconIndex;

			if (!UIManager.Instance.OptionUI.MenuParent.gameObject.activeSelf) return;
			HandleIconInitials();
		}

		/// <summary>
		/// Check for new input device and change icon initials for it.
		/// This only checks for the registered actions in action input. 
		/// Does nothing for other actions.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="change"></param>
		void HandleInputDeviceType(object obj, InputActionChange change)
		{
			if (change != InputActionChange.ActionStarted) return;

			// Get the last input device.
			var lastControl = ((InputAction)obj).activeControl;

			// Return if it's the same.
			if (lastControl.device == previousDevice) return;
			previousDevice = lastControl.device;

			HandleInputDeviceCompare(lastControl.device.displayName);
		}

		/// <summary>
		/// Check the name of the current device to get correct device.
		/// </summary>
		/// <param name="currentDeviceName"></param>
		void HandleInputDeviceCompare(string currentDeviceName)
		{
			// Get the input type.
			InputDeviceType inputType = InputDeviceType.Gamepad;
			if (Equals(currentDeviceName, "Mouse") || Equals(currentDeviceName, "Keyboard"))
			{
				inputType = InputDeviceType.KeyboardMouse;
			}

			// Since mouse and keyboard are treated as different entity, return if it's the same.
			if (InputDeviceType == inputType) return;

			InputDeviceType = inputType;
			HandleIconInitials();

			Debug.Log($"DeviceType : {InputDeviceType}");
		}

		/// <summary>
		/// Handle the control initials. Ex: DS4_/XBox_/KM_ etc for display of icons.
		/// </summary>
		void HandleIconInitials()
		{
			if (gamepadIconIndex == 1) // Keyboard
			{
				SetInitials(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}
			else if (gamepadIconIndex == 2) // Dualshock
			{
				SetInitials(IconDisplayType.Dualshock.GetStringValue());
				return;
			}
			else if (gamepadIconIndex == 3) // XBox
			{
				SetInitials(IconDisplayType.Xbox.GetStringValue());
				return;
			}

			// Auto check
			if (InputDeviceType == InputDeviceType.KeyboardMouse)
			{
				SetInitials(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}

			// Check for gamepad...
			if (SetInitialsWhenGamepadContains("DualShock", IconDisplayType.Dualshock.GetStringValue())) return;
			else if (SetInitialsWhenGamepadContains("XBox", IconDisplayType.Xbox.GetStringValue())) return;
		}

		bool SetInitialsWhenGamepadContains(string subset, string initials)
		{
			if (Gamepad.current.device.name.Contains(subset, StringComparison.OrdinalIgnoreCase))
			{
				SetInitials(initials);
				return true;
			}
			return false;
		}

		void SetInitials(string initials)
		{
			IconInitials = initials;
			OnDeviceIconChanged?.Invoke();
		}

		void OnDestroy()
		{
			InputSystem.onActionChange -= HandleInputDeviceType;
		}
	}
}

