using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using Personal.GameState;
using Personal.InputProcessing;
using Personal.Definition;
using Helper;
using static Personal.Definition.InputReaderDefinition;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem.UI;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
		[SerializeField] InputReaderDefinition inputReaderDefinition = null;
		[SerializeField] ButtonIconDefinition buttonIconDefinition = null;
		[SerializeField] [ReadOnly] ActionMapType currentActionMapType = ActionMapType.BasicControl;

		public InputReaderDefinition InputReaderDefinition { get => inputReaderDefinition; }
		public PlayerActionInput PlayerActionInput { get; private set; }
		public ButtonIconDefinition ButtonIconDefinition { get => buttonIconDefinition; }

		// Different actions maps for different situations.
		public BasicControlInputController BasicControlInputController { get; private set; }
		public FPSInputController FPSInputController { get; private set; }
		public UIInputController UIInputController { get; private set; }
		public PuzzleInputController PuzzleInputController { get; private set; }

		public ActionMapType CurrentActionMapType { get => currentActionMapType; }
		public InputDeviceType InputDeviceType { get; private set; } = InputDeviceType.None;

		public bool IsInteract
		{
			get
			{
				switch (currentActionMapType)
				{
					case ActionMapType.Player: return FPSInputController.IsInteract;
					case ActionMapType.UI: return UIInputController.IsInteract;
					case ActionMapType.Puzzle: return PuzzleInputController.IsInteract;
					default: return BasicControlInputController.IsInteract;
				}
			}
		}

		public bool IsCancel
		{
			get
			{
				switch (currentActionMapType)
				{
					case ActionMapType.Player: return FPSInputController.IsCancel;
					case ActionMapType.UI: return UIInputController.IsCancel;
					case ActionMapType.Puzzle: return PuzzleInputController.IsCancel;
					default: return BasicControlInputController.IsInteract;
				}
			}
		}

		public string IconInitials { get; private set; }
		public Gamepad CurrentGamepad { get; private set; }

		public event Action OnAnyButtonPressed;
		public event Action OnDeviceIconChanged;

		InputSystemUIInputModule inputSystemUIInputModule;
		InputActionReference submitActionReference;
		InputActionReference cancelActionReference;

		InputDevice previousDevice = null;
		IconDisplayType iconDisplayType;

		IDisposable iDisposableAnyButtonPressed;

		protected override void Initialize()
		{
			BasicControlInputController = GetComponentInChildren<BasicControlInputController>(true);
			FPSInputController = GetComponentInChildren<FPSInputController>(true);
			UIInputController = GetComponentInChildren<UIInputController>(true);
			PuzzleInputController = GetComponentInChildren<PuzzleInputController>(true);
			inputSystemUIInputModule = GetComponentInChildren<InputSystemUIInputModule>(true);

			submitActionReference = inputSystemUIInputModule.submit;
			cancelActionReference = inputSystemUIInputModule.cancel;

			inputReaderDefinition.Initialize();
			SetToDefaultActionMap();

			InputSystem.onActionChange += HandleInputDeviceType;
			iDisposableAnyButtonPressed = InputSystem.onAnyButtonPress.Call(ctrl =>
			{
				OnAnyButtonPressed?.Invoke();
				HandleInputDeviceCompare(ctrl.device);
			});
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
		}

		/// <summary>
		/// Reset back to default action map.
		/// </summary>
		public void SetToDefaultActionMap()
		{
			ActionMapType defaultActionMap = ActionMapType.BasicControl;
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
			HandleIconInitials();
		}

		/// <summary>
		/// Used when changing the interact button between x/o and a/b.
		/// </summary>
		/// <param name="isUSInteract"></param>
		public void SwapInteractInput(bool isUSInteract)
		{
			inputReaderDefinition.SwapInteractInput(isUSInteract);

			if (isUSInteract)
			{
				inputSystemUIInputModule.submit = submitActionReference;
				inputSystemUIInputModule.cancel = cancelActionReference;
				return;
			}

			inputSystemUIInputModule.submit = cancelActionReference;
			inputSystemUIInputModule.cancel = submitActionReference;
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
			if (inputDevice.name.Equals("Mouse") || inputDevice.name.Equals("Keyboard"))
			{
				inputType = InputDeviceType.KeyboardMouse;
			}

			if (inputType == InputDeviceType) return;

			// Check for gamepads.
			InputDeviceType = inputType;
			previousDevice = inputDevice;

			HandleCurrentGamepad(inputDevice.name);
			HandleIconInitials();

			Debug.Log("DeviceType : " + (InputDeviceType == InputDeviceType.KeyboardMouse ? InputDeviceType.ToString() : inputDevice.name));
		}

		/// <summary>
		/// Make sure the active gamepad is registered.
		/// Gamepad.current does not always return the used, active gamepad.
		/// </summary>
		/// <param name="currentDeviceName"></param>
		void HandleCurrentGamepad(string currentDeviceName)
		{
			CurrentGamepad = null;
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
			if (iconDisplayType == IconDisplayType.KeyboardMouse)
			{
				SetInitials(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}
			else if (iconDisplayType == IconDisplayType.Dualshock)
			{
				SetInitials(IconDisplayType.Dualshock.GetStringValue());
				return;
			}
			else if (iconDisplayType == IconDisplayType.Xbox)
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

			if (CurrentGamepad == null) return;

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

		void OnApplicationQuit()
		{
			InputSystem.onActionChange -= HandleInputDeviceType;
			iDisposableAnyButtonPressed.Dispose();
		}
	}
}

