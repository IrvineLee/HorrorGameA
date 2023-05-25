using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

using Personal.GameState;
using Personal.InputProcessing;
using Personal.Definition;
using Helper;

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

		InputDevice currentDevice = null;
		InputDevice previousDevice = null;

		protected override async UniTask Awake()
		{
			await base.Awake();

			FPSInputController = GetComponentInChildren<FPSInputController>();
			UIInputController = GetComponentInChildren<UIInputController>();
			PuzzleInputController = GetComponentInChildren<PuzzleInputController>();

			inputReaderDefinition.Initialize();
			SetToDefaultActionMap();

			InputSystem.onActionChange += HandleInputDeviceType;
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
		/// Check for new input device and change icon initials for it.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="change"></param>
		void HandleInputDeviceType(object obj, InputActionChange change)
		{
			if (change != InputActionChange.ActionStarted) return;

			// Get the last input device.
			var lastControl = ((InputAction)obj).activeControl;
			currentDevice = lastControl.device;

			// Return if it's the same.
			if (currentDevice == previousDevice) return;
			previousDevice = currentDevice;

			// Get the input type.
			InputDeviceType inputType = InputDeviceType.Gamepad;
			if (Equals(currentDevice.displayName, "Mouse") || Equals(currentDevice.displayName, "Keyboard"))
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
			if (InputDeviceType == InputDeviceType.KeyboardMouse)
			{
				IconInitials = IconDisplayType.KeyboardMouse.GetStringValue();
				OnDeviceIconChanged?.Invoke();
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
				IconInitials = initials;
				OnDeviceIconChanged?.Invoke();
				return true;
			}
			return false;
		}

		void OnDestroy()
		{
			InputSystem.onActionChange -= HandleInputDeviceType;
		}
	}
}

