using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Personal.GameState;
using Personal.InputProcessing;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class InputManager : GameInitializeSingleton<InputManager>
	{
		[SerializeField] InputReaderDefinition inputReaderDefinition = null;
		[SerializeField] ActionMapType defaultActionMap = ActionMapType.Player;

		public InputReaderDefinition InputReaderDefinition { get => inputReaderDefinition; }
		public PlayerActionInput PlayerActionInput { get; private set; }

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

		public void SetToDefaultActionMap()
		{
			EnableActionMap(defaultActionMap);
		}

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
			Debug.Log($"DeviceType : {InputDeviceType}");
		}

		void OnDestroy()
		{
			InputSystem.onActionChange -= HandleInputDeviceType;
		}
	}
}

