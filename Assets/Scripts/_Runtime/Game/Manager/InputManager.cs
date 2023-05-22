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
		[SerializeField] InputReader inputReader = null;
		[SerializeField] ActionMapType defaultActionMap = ActionMapType.Player;

		public InputReader InputReader { get => inputReader; }
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

			inputReader.Initialize();
			EnableActionMap(defaultActionMap);

			InputSystem.onActionChange += HandleInputDeviceType;
		}

		public void EnableActionMap(ActionMapType actionMap)
		{
			// Disable all action map.
			foreach (var map in inputReader.InputActionMapDictionary)
			{
				map.Value.InputActionMap.Disable();
				map.Value.InputController.enabled = false;
			}

			// Enable specified actin map.
			inputReader.InputActionMapDictionary.TryGetValue(actionMap, out var inputActionMap);
			inputActionMap.InputActionMap.Enable();
			inputActionMap.InputController.enabled = true;

			CurrentActionMapType = actionMap;
		}

		public void ResetToDefaultActionMap()
		{
			EnableActionMap(defaultActionMap);
		}

		void HandleInputDeviceType(object obj, InputActionChange change)
		{
			if (change != InputActionChange.ActionStarted) return;

			var lastControl = ((InputAction)obj).activeControl;
			currentDevice = lastControl.device;

			if (currentDevice == previousDevice) return;
			previousDevice = currentDevice;

			InputDeviceType inputType = InputDeviceType.Gamepad;
			if (Equals(currentDevice.displayName, "Mouse") || Equals(currentDevice.displayName, "Keyboard"))
			{
				inputType = InputDeviceType.KeyboardMouse;
			}

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

