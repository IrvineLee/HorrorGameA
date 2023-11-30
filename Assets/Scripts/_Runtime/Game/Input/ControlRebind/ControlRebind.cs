using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

using Helper;
using Personal.Manager;
using Personal.GameState;
using Personal.InputProcessing;
using Lean.Localization;

namespace Personal.UI
{
	public class ControlRebind : GameInitialize
	{
		[SerializeField] MenuUI waitingForInputMenu = null;
		[SerializeField] List<string> cancelThroughStrList = null;

		public event Action OnRemapped;

		RebindingOperation rebindingOperation;

		UISelectionSubmit_ControlRebind currentSelection;
		InputAction inputAction;
		List<string> overridePathList = new();

		protected override void OnEnabled()
		{
			InputManager.OnDeviceDisconnected += OnDeviceDisconnected;
		}

		public void StartRebind(UISelectionSubmit_ControlRebind uiSelectionSubmit)
		{
			currentSelection = uiSelectionSubmit;
			inputAction = currentSelection.InputAction;

			waitingForInputMenu.OpenWindow();
			InputManager.Instance.DisableAllActionMap();

			InitRebind();
		}

		void InitRebind()
		{
			UISelectable.LockSelection(true);

			// Set the cancel token.
			string inputDeviceTypeStr = "Keyboard";
			string cancelStr = "Esc";

			InputDeviceType inputDeviceType = currentSelection.InputDeviceTypeSet.InputDeviceType;
			if (inputDeviceType == InputDeviceType.Gamepad)
			{
				// Get from InputManager to see whether it's gamepad/joystick. Assume it's a gamepad when user start with mouse.
				// It is only a joystick when you start it with the joystick.
				inputDeviceTypeStr = InputManager.Instance.IsCurrentDeviceMouse ? "Gamepad" : InputManager.Instance.InputDeviceType.ToString();
				cancelStr = (currentSelection.IconInitials + GenericButtonIconType.Button_Option).SpriteEnclose();
			}
			LeanLocalization.SetToken("CANCEL", cancelStr);

			// Stash the overridePath to reset it in case of cancelling through with different keys.
			overridePathList.Clear();
			foreach (var binding in inputAction.bindings)
			{
				overridePathList.Add(binding.overridePath);
			}

			// Rebinding operation.
			rebindingOperation = inputAction.PerformInteractiveRebinding()
				.WithControlsExcluding("Mouse")
				.WithBindingGroup(inputDeviceTypeStr)                                       // Makes sure to only override 1 binding in an action
				.WithControlsHavingToMatchPath("<" + inputDeviceTypeStr + ">")              // Makes sure to only allow input from specific device/control
				.OnMatchWaitForAnother(0.1f)
				.WithCancelingThrough("<Keyboard>/escape")                                  // This only allow for 1 cancel key. Other keys are checked within RebindComplete
				.OnCancel((operation) => EndRebind(false))
				.OnComplete((operation) => RebindComplete(operation))
				.Start();
		}

		void RebindComplete(RebindingOperation operation)
		{
			// This gets the pressed button input for the correct device index.
			int bindingIndex = inputAction.GetBindingIndexForControl(operation.selectedControl);
			var binding = inputAction.bindings[bindingIndex];
			var humanReadableType = InputControlPath.HumanReadableStringOptions.OmitDevice;

			Debug.Log("binding.overridePath " + binding.overridePath);

			bool isOverriden = false;
			if (!IsCancellingThrough(binding))
			{
				currentSelection.NameTMP.text = InputControlPath.ToHumanReadableString(binding.effectivePath, humanReadableType);
				isOverriden = true;
			}

			foreach (var b in inputAction.bindings)
			{
				Debug.Log("Binding  " + b);
			}

			EndRebind(isOverriden);
		}

		bool IsCancellingThrough(InputBinding binding)
		{
			// If it's not part of keyboard or gamepad, assume it's a joystick.
			string overridePath = binding.overridePath;
			if (!overridePath.Contains("Keyboard") && !overridePath.Contains("Gamepad"))
			{
				overridePath = "<Joystick>" + overridePath.Substring(overridePath.IndexOf('>') + 1);
			}

			foreach (var str in cancelThroughStrList)
			{
				if (!string.Equals(str, overridePath)) continue;

				// Reset the override.
				for (int i = 0; i < overridePathList.Count; i++)
				{
					InputActionRebindingExtensions.ApplyBindingOverride(inputAction, i, new InputBinding { overridePath = overridePathList[i] });
				}
				return true;
			}

			return false;
		}

		void EndRebind(bool isOverriden)
		{
			rebindingOperation.Dispose();

			// If the user cancelled the process(ESC), UIManager will handle the closing of window.
			// If rebind is successful or using gamepad, close the window here. (ESC key also closes other window)
			if (isOverriden || !InputManager.Instance.IsCurrentDeviceMouse) waitingForInputMenu.CloseWindow();

			InputManager.Instance.EnableActionMap(ActionMapType.UI);
			UISelectable.LockSelection(false);
			OnRemapped?.Invoke();

			Debug.Log("REBIND End! " + isOverriden);
		}

		void OnDeviceDisconnected(InputDevice inputDevice)
		{
			// TODO: UI appearing showing a device has been disconnected.
			rebindingOperation.Reset();
		}

		protected override void OnDisabled()
		{
			InputManager.OnDeviceDisconnected -= OnDeviceDisconnected;
		}
	}
}