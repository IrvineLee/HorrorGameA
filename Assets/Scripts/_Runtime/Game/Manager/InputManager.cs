using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;

using Sirenix.OdinInspector;
using Helper;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.Definition;
using Personal.Setting.Game;
using Personal.UI.Option;
using static Personal.Definition.InputReaderDefinition;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
		public enum MotionType
		{
			Move = 0,
			LookAt,
		}

		public enum ButtonPush
		{
			Submit = 0,
			Cancel,
		}

		[SerializeField] InputReaderDefinition inputReaderDefinition = null;
		[SerializeField] ButtonIconDefinition buttonIconDefinition = null;
		[SerializeField] [ReadOnly] ActionMapType currentActionMapType = ActionMapType.UI;

		public InputReaderDefinition InputReaderDefinition { get => inputReaderDefinition; }
		public PlayerActionInput PlayerActionInput { get => inputReaderDefinition.PlayerActionInput; }
		public ButtonIconDefinition ButtonIconDefinition { get => buttonIconDefinition; }

		// Different actions maps for different situations.
		public FPSInputController FPSInputController { get; private set; }
		public UIInputController UIInputController { get; private set; }
		public PuzzleInputController PuzzleInputController { get; private set; }

		public ActionMapType CurrentActionMapType { get => currentActionMapType; }
		public InputDeviceType InputDeviceType { get; private set; } = InputDeviceType.None;
		public bool IsCurrentDeviceMouse { get => InputDeviceType == InputDeviceType.KeyboardMouse; }
		public bool IsChangeIconOnly { get; private set; }

		public string IconInitials { get; private set; }                    // This gets the currently active device (KeyboardMouse, DS4, XBox, etc)
		public string KeyboardIconInitials { get; private set; } = "KM_";   // This always gets the keyboard icon initials, doesn't matter if active or not.
		public string GamepadIconInitials { get; private set; }             // This always gets the gamepad/joystick icon initials, doesn't matter if active or not.
		public Gamepad CurrentGamepad { get; private set; }                 // This is always the gamepad with rumbling function.

		public static event Action OnAnyButtonPressed;
		public static event Action<InputDevice> OnDeviceDisconnected;
		public static event Action OnDeviceIconChanged;

		string currentControllerName;

		InputSystemUIInputModule inputSystemUIInputModule;
		InputActionReference submitActionReference;
		InputActionReference cancelActionReference;

		InputDevice previousDevice = null;
		IconDisplayType iconDisplayType;

		IDisposable iDisposableAnyButtonPressed;

		GameData gameData;

		protected override void Initialize()
		{
			FPSInputController = GetComponentInChildren<FPSInputController>(true);
			UIInputController = GetComponentInChildren<UIInputController>(true);
			PuzzleInputController = GetComponentInChildren<PuzzleInputController>(true);
			inputSystemUIInputModule = GetComponentInChildren<InputSystemUIInputModule>(true);

			GamepadIconInitials = IconDisplayType.Xbox.GetStringValue();
			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;

			submitActionReference = inputSystemUIInputModule.submit;
			cancelActionReference = inputSystemUIInputModule.cancel;

			inputReaderDefinition.Initialize();

			InputSystem.onActionChange += HandleInputDeviceType;
			InputSystem.onDeviceChange += HandleDisconnectedDevice;

			iDisposableAnyButtonPressed = InputSystem.onAnyButtonPress.Call(ctrl =>
			{
				OnAnyButtonPressed?.Invoke();
				HandleInputDeviceCompare(ctrl.device);
			});

			OptionGameUI.OnXInteractEvent += SwapInteractInput;
		}

		/// <summary>
		/// Get the current active motion.
		/// </summary>
		/// <param name="motionType"></param>
		/// <param name="isAffectedByOption">Return value that is affected by invert controls</param>
		/// <returns></returns>
		public Vector3 GetMotion(MotionType motionType, bool isAffectedByOption = false)
		{
			Vector3 movement;
			switch (currentActionMapType)
			{
				case ActionMapType.Player: movement = motionType == MotionType.Move ? FPSInputController.Move : FPSInputController.Look; break;
				case ActionMapType.UI: movement = motionType == MotionType.Move ? UIInputController.Move : UIInputController.Look; break;
				case ActionMapType.Puzzle: movement = motionType == MotionType.Move ? PuzzleInputController.Move : PuzzleInputController.Look; break;

				default: return Vector3.zero;
			}

			if (!isAffectedByOption) return movement;

			if (gameData.IsInvertLookHorizontal) movement.x = -movement.x;
			if (gameData.IsInvertLookVertical) movement.y = -movement.y;

			return movement;
		}

		public bool GetButtonPush(ButtonPush buttonPush)
		{
			switch (currentActionMapType)
			{
				case ActionMapType.Player: return buttonPush == ButtonPush.Submit ? FPSInputController.IsInteract : FPSInputController.IsCancel;
				case ActionMapType.UI: return buttonPush == ButtonPush.Submit ? UIInputController.IsInteract : UIInputController.IsCancel;
				case ActionMapType.Puzzle: return buttonPush == ButtonPush.Submit ? PuzzleInputController.IsInteract : PuzzleInputController.IsCancel;

				default: return false;
			}
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
			DisableAllActionMap();
			if (actionMap == ActionMapType.None) return;

			// Enable specified action map.
			var inputControllerInfo = GetInputControllerInfo(actionMap);
			inputControllerInfo.Enable(true);

			currentActionMapType = actionMap;
		}

		/// <summary>
		/// Disable all action map. 
		/// </summary>
		public void DisableAllActionMap()
		{
			foreach (var map in inputReaderDefinition.InputActionMapDictionary)
			{
				map.Value.Enable(false);
			}

			currentActionMapType = ActionMapType.None;
		}

		/// <summary>
		/// Reset back to default action map.
		/// </summary>
		public void SetToDefaultActionMap()
		{
			ActionMapType defaultActionMap = ActionMapType.UI;
			if (GameSceneManager.Instance.IsMainScene())
			{
				defaultActionMap = ActionMapType.Player;
			}

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

			IsChangeIconOnly = true;
			HandleIconInitials();
			IsChangeIconOnly = false;
		}

		/// <summary>
		/// Handle disconnected device.
		/// </summary>
		/// <param name="inputDevice"></param>
		/// <param name="inputDeviceChange"></param>
		void HandleDisconnectedDevice(InputDevice inputDevice, InputDeviceChange change)
		{
			if (change != InputDeviceChange.Disconnected) return;
			OnDeviceDisconnected?.Invoke(inputDevice);
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

			// Handle input device compare.
			var lastControl = ((InputAction)obj).activeControl;
			HandleInputDeviceCompare(lastControl.device);
		}

		/// <summary>
		/// Check the name of the current device to get correct device.
		/// </summary>
		/// <param name="inputDevice"></param>
		void HandleInputDeviceCompare(InputDevice inputDevice)
		{
			// Return if it's the same device.
			if (previousDevice == inputDevice) return;

			// Get the input type.
			InputDeviceType inputType = GetCurrentController(inputDevice.name);
			if (inputDevice.name.ContainsIgnoreCase("Mouse") || inputDevice.name.ContainsIgnoreCase("Keyboard"))
			{
				inputType = InputDeviceType.KeyboardMouse;
			}

			if (InputDeviceType != inputType ||
				(inputType != InputDeviceType.KeyboardMouse && previousDevice != inputDevice))
			{
				InputDeviceType = inputType;
				previousDevice = inputDevice;

				HandleIconInitials();
				Debug.Log("DeviceType : " + (InputDeviceType == InputDeviceType.KeyboardMouse ? InputDeviceType.ToString() : inputDevice.name));
			}
		}

		/// <summary>
		/// Get the current gamepad/joystick.
		/// </summary>
		/// <param name="currentDeviceName"></param>
		InputDeviceType GetCurrentController(string currentDeviceName)
		{
			CurrentGamepad = null;
			currentControllerName = "";

			InputDeviceType inputDeviceType = UpdateCurrentController(currentDeviceName, Gamepad.all);
			if (inputDeviceType == InputDeviceType.None) inputDeviceType = UpdateCurrentController(currentDeviceName, Joystick.all);

			return inputDeviceType;
		}

		/// <summary>
		/// Check between gamepad and joystick.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="currentDeviceName"></param>
		/// <param name="array"></param>
		InputDeviceType UpdateCurrentController<T>(string currentDeviceName, ReadOnlyArray<T> array) where T : InputDevice
		{
			foreach (var controller in array)
			{
				if (!controller.name.Contains(currentDeviceName)) continue;

				currentControllerName = controller.name;
				if (typeof(T) == typeof(Gamepad))
				{
					CurrentGamepad = (Gamepad)(object)controller;
					return InputDeviceType.Gamepad;
				}
				else
				{
					return InputDeviceType.Joystick;
				}
			}
			return InputDeviceType.None;
		}

		/// <summary>
		/// Handle the control initials. Ex: DS4_/XBox_/KM_ etc for display of icons.
		/// </summary>
		void HandleIconInitials()
		{
			if (iconDisplayType == IconDisplayType.KeyboardMouse || iconDisplayType == IconDisplayType.Dualshock ||
			iconDisplayType == IconDisplayType.Xbox || iconDisplayType == IconDisplayType.NintendoSwitch)
			{
				SetInitialsAndHandleGamepadInteractInput(iconDisplayType.GetStringValue());
				return;
			}

			// All condition-checks below are auto check...
			if (InputDeviceType == InputDeviceType.KeyboardMouse)
			{
				SetInitialsAndHandleGamepadInteractInput(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}

			if (string.IsNullOrEmpty(currentControllerName)) return;

			// Check for gamepad/joystick...
			if (SetInitialsWhenGamepadContains("DualShock", IconDisplayType.Dualshock.GetStringValue())) return;
			else if (SetInitialsWhenGamepadContains("DualSense", IconDisplayType.Dualshock.GetStringValue())) return;
			else if (SetInitialsWhenGamepadContains("SwitchProController", IconDisplayType.NintendoSwitch.GetStringValue())) return;

			// When all else fail, revert back to XBox layout.
			SetInitialsAndHandleGamepadInteractInput(IconDisplayType.Xbox.GetStringValue());
		}

		bool SetInitialsWhenGamepadContains(string subset, string initials)
		{
			if (currentControllerName.Contains(subset, StringComparison.OrdinalIgnoreCase))
			{
				SetInitialsAndHandleGamepadInteractInput(initials);
				return true;
			}
			return false;
		}

		void SetInitialsAndHandleGamepadInteractInput(string initials)
		{
			IconInitials = initials;
			if (!initials.Contains(IconDisplayType.KeyboardMouse.GetStringValue())) GamepadIconInitials = initials;

			OnDeviceIconChanged?.Invoke();

			// You don't wanna swap the interact input when in mouse mode.
			if (IsCurrentDeviceMouse)
			{
				SwapInteractInput(true);
				return;
			}
			SwapInteractInput(gameData.IsXInteractButton);
		}

		/// <summary>
		/// Used when changing the interact button between x/o and a/b.
		/// </summary>
		/// <param name="isXInteract"></param>
		void SwapInteractInput(bool isXInteract)
		{
			inputReaderDefinition.SwapInteractInput(isXInteract);

			if (isXInteract)
			{
				inputSystemUIInputModule.submit = submitActionReference;
				inputSystemUIInputModule.cancel = cancelActionReference;
				return;
			}

			inputSystemUIInputModule.submit = cancelActionReference;
			inputSystemUIInputModule.cancel = submitActionReference;
		}

		void OnApplicationQuit()
		{
			InputSystem.onActionChange -= HandleInputDeviceType;
			InputSystem.onDeviceChange -= HandleDisconnectedDevice;

			iDisposableAnyButtonPressed?.Dispose();
			OptionGameUI.OnXInteractEvent -= SwapInteractInput;
		}
	}
}