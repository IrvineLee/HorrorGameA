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
using static Personal.Definition.InputReaderDefinition;

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
		public InputDeviceType InputDeviceType { get; private set; } = InputDeviceType.KeyboardMouse;

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
		public Gamepad CurrentGamepad { get; private set; }

		public event Action OnDeviceIconChanged;

		InputDevice previousDevice = null;
		IconDisplayType iconDisplayType;

		string currentDeviceName;

		protected override async UniTask Awake()
		{
			await base.Awake();

			FPSInputController = GetComponentInChildren<FPSInputController>();
			UIInputController = GetComponentInChildren<UIInputController>();
			PuzzleInputController = GetComponentInChildren<PuzzleInputController>();

			inputReaderDefinition.Initialize();
			SetToDefaultActionMap();

			InputSystem.onActionChange += HandleInputDeviceType;
			InputSystem.onAnyButtonPress.Call(ctrl => HandleInputDeviceCompare(ctrl.device));
		}

		/// <summary>
		/// Get input controller info.
		/// </summary>
		/// <param name="actionMap"></param>
		/// <returns></returns>
		public InputControllerInfo GetInputControllerInfo(ActionMapType actionMap)
		{
			inputReaderDefinition.InputActionMapDictionary.TryGetValue(actionMap, out var inputActionMap);
			return inputActionMap;
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
				map.Value.Enable(false);
			}

			// Enable specified action map.
			var inputControllerInfo = GetInputControllerInfo(actionMap);
			inputControllerInfo.Enable(true);

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
		public void SetGamepadIconIndex(IconDisplayType iconDisplayType)
		{
			this.iconDisplayType = iconDisplayType;

			if (!UIManager.Instance.OptionUI.gameObject.activeSelf) return;
			HandleIconInitials();
		}

		/// <summary>
		/// Typically used for registered actions. 
		/// This is used for analog confirmation.
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

			HandleInputDeviceCompare(lastControl.device);
		}

		/// <summary>
		/// Check the name of the current device to get correct device.
		/// </summary>
		/// <param name="inputDevice"></param>
		void HandleInputDeviceCompare(InputDevice inputDevice)
		{
			if (previousDevice == inputDevice) return;

			// Get the input type.
			InputDeviceType inputType = InputDeviceType.Gamepad;
			if (Equals(inputDevice.name, "Mouse") || Equals(inputDevice.name, "Keyboard"))
			{
				inputType = InputDeviceType.KeyboardMouse;
			}

			// Check for gamepads.
			InputDeviceType = inputType;
			previousDevice = inputDevice;

			HandleCurrentGamepad(inputDevice.name);
			HandleIconInitials();

			Debug.Log($"DeviceType : {inputDevice.name}");
		}

		/// <summary>
		/// Make sure the active gamepad is registered.
		/// Gamepad.current does not always return the used, active gamepad.
		/// </summary>
		/// <param name="currentDeviceName"></param>
		void HandleCurrentGamepad(string currentDeviceName)
		{
			foreach (var gamepad in Gamepad.all)
			{
				if (gamepad.name.Contains(currentDeviceName))
				{
					CurrentGamepad = gamepad;
					break;
				}
			}
		}

		/// <summary>
		/// Handle the control initials. Ex: DS4_/XBox_/KM_ etc for display of icons.
		/// </summary>
		void HandleIconInitials()
		{
			if (iconDisplayType == IconDisplayType.KeyboardMouse) // Keyboard
			{
				SetInitials(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}
			else if (iconDisplayType == IconDisplayType.Dualshock) // Dualshock
			{
				SetInitials(IconDisplayType.Dualshock.GetStringValue());
				return;
			}
			else if (iconDisplayType == IconDisplayType.Xbox) // XBox
			{
				SetInitials(IconDisplayType.Xbox.GetStringValue());
				return;
			}

			// All condition-checks below are auto check...
			if (InputDeviceType == InputDeviceType.KeyboardMouse)
			{
				SetInitials(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}

			// Check for gamepad...
			if (SetInitialsWhenGamepadContains("DualShock", IconDisplayType.Dualshock.GetStringValue())) return;
			else if (SetInitialsWhenGamepadContains("DualSense", IconDisplayType.Dualshock.GetStringValue())) return;
			else if (SetInitialsWhenGamepadContains("XBox", IconDisplayType.Xbox.GetStringValue())) return;
		}

		bool SetInitialsWhenGamepadContains(string subset, string initials)
		{
			if (CurrentGamepad.name.Contains(subset, StringComparison.OrdinalIgnoreCase))
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

