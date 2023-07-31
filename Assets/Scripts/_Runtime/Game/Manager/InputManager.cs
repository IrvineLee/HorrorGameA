using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;

using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.InputProcessing;
using Personal.Definition;
using Personal.Setting.Game;
using Helper;
using static Personal.Definition.InputReaderDefinition;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
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

		public Vector3 Move
		{
			get
			{
				switch (currentActionMapType)
				{
					case ActionMapType.Player: return FPSInputController.Move;
					case ActionMapType.Puzzle: return PuzzleInputController.Move;
					default: return UIInputController.Move;
				}
			}
		}

		public bool IsInteract
		{
			get
			{
				switch (currentActionMapType)
				{
					case ActionMapType.Player: return FPSInputController.IsInteract;
					case ActionMapType.Puzzle: return PuzzleInputController.IsInteract;
					default: return UIInputController.IsInteract;
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
					case ActionMapType.Puzzle: return PuzzleInputController.IsCancel;
					default: return UIInputController.IsCancel;
				}
			}
		}

		public string IconInitials { get; private set; }
		public Gamepad CurrentGamepad { get; private set; }

		public static event Action OnAnyButtonPressed;
		public static event Action OnDeviceIconChanged;

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

			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;

			submitActionReference = inputSystemUIInputModule.submit;
			cancelActionReference = inputSystemUIInputModule.cancel;

			inputReaderDefinition.Initialize();

			InputSystem.onActionChange += HandleInputDeviceType;
			iDisposableAnyButtonPressed = InputSystem.onAnyButtonPress.Call(ctrl =>
			{
				OnAnyButtonPressed?.Invoke();
				HandleInputDeviceCompare(ctrl.device);
			});
		}

		protected override async void OnMainScene()
		{
			DisableAllActionMap();
			await UniTask.WaitUntil(() => !StageManager.Instance.IsBusy);
			EnableActionMap(ActionMapType.Player);
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
			// Return if it's the same.
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
				SetInitialsAndHandleGamepadInteractInput(IconDisplayType.KeyboardMouse.GetStringValue());
				return;
			}
			else if (iconDisplayType == IconDisplayType.Dualshock)
			{
				SetInitialsAndHandleGamepadInteractInput(IconDisplayType.Dualshock.GetStringValue());
				return;
			}
			else if (iconDisplayType == IconDisplayType.Xbox)
			{
				SetInitialsAndHandleGamepadInteractInput(IconDisplayType.Xbox.GetStringValue());
				return;
			}

			// All condition-checks below are auto check...
			if (InputDeviceType == InputDeviceType.KeyboardMouse)
			{
				SetInitialsAndHandleGamepadInteractInput(IconDisplayType.KeyboardMouse.GetStringValue());
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
				SetInitialsAndHandleGamepadInteractInput(initials);
				return true;
			}
			return false;
		}

		void SetInitialsAndHandleGamepadInteractInput(string initials)
		{
			IconInitials = initials;
			OnDeviceIconChanged?.Invoke();

			// You don't wanna swap the interact input when in mouse mode.
			if (IsCurrentDeviceMouse)
			{
				SwapInteractInput(true);
				return;
			}
			SwapInteractInput(gameData.IsUSInteractButton);
		}

		void OnApplicationQuit()
		{
			InputSystem.onActionChange -= HandleInputDeviceType;
			iDisposableAnyButtonPressed.Dispose();
		}
	}
}