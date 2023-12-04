using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

using Helper;
using Lean.Localization;
using Personal.Manager;
using Personal.GameState;
using Personal.InputProcessing;

namespace Personal.UI
{
	public class ControlRebind : GameInitialize
	{
		[SerializeField] MenuUI waitingForInputMenu = null;
		[SerializeField] List<string> cancelThroughStrList = null;
		[SerializeField] List<string> ignoreThroughStrList = null;

		public InputActionMap InputActionMap { get; private set; }

		public event Action OnRebinded;

		RebindingOperation rebindingOperation;

		UISelectionSubmit_ControlRebind currentSelection;
		InputAction inputAction;

		List<string> overridePathList = new();
		string previousPath;

		protected override void OnEnabled()
		{
			InputManager.OnDeviceDisconnected += OnDeviceDisconnected;
		}

		public void InitialSetup()
		{
			InputActionMap = InputManager.Instance.PlayerActionInput.asset.FindActionMap("Player");
		}

		/// <summary>
		/// Start the rebind process.
		/// </summary>
		/// <param name="uiSelectionSubmit"></param>
		/// <param name="compositeBindIndex">If it's not part of composite binding, you can safely ignore this.</param>
		public void StartRebind(UISelectionSubmit_ControlRebind uiSelectionSubmit, int compositeBindIndex = -1)
		{
			currentSelection = uiSelectionSubmit;
			inputAction = currentSelection.InputAction;

			string overridePath = currentSelection.InputBinding.overridePath;
			previousPath = string.IsNullOrEmpty(overridePath) ? currentSelection.InputBinding.path : overridePath;

			if (compositeBindIndex >= 0)
			{
				var inputBind = inputAction.bindings[compositeBindIndex];
				previousPath = string.IsNullOrEmpty(inputBind.overridePath) ? inputBind.path : inputBind.overridePath;
			}

			waitingForInputMenu.OpenWindow();
			InputManager.Instance.DisableAllActionMap();

			InitRebind(compositeBindIndex);
		}

		void InitRebind(int compositeBindIndex = -1)
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
				.OnComplete((operation) => RebindComplete(operation, compositeBindIndex));

			if (compositeBindIndex >= 0) rebindingOperation.WithTargetBinding(compositeBindIndex);
			rebindingOperation.Start();
		}

		/// <summary>
		/// Rebind is done.
		/// </summary>
		/// <param name="operation"></param>
		void RebindComplete(RebindingOperation operation, int compositeBindIndex = -1)
		{
			// This gets the pressed button input for the correct device index.
			int bindingIndex = inputAction.GetBindingIndexForControl(operation.selectedControl);
			var binding = inputAction.bindings[bindingIndex];
			var humanReadableType = InputControlPath.HumanReadableStringOptions.OmitDevice;

			Debug.Log("--------Action : " + binding.action + " Path : " + binding.path + " Eff Path : " + binding.effectivePath + " Over Path : " + binding.overridePath);

			// Check for ignore bindings.
			if (IsIgnoreThrough(binding))
			{
				InputActionRebindingExtensions.ApplyBindingOverride(inputAction, bindingIndex, new InputBinding { overridePath = previousPath });
				InitRebind(compositeBindIndex);
				return;
			}

			// Check for cancel bindings.
			bool isOverriden = false;
			if (!IsCancellingThrough(binding))
			{
				currentSelection.NameTMP.text = InputControlPath.ToHumanReadableString(binding.effectivePath, humanReadableType);
				isOverriden = true;

				HandleSwapBinding(binding);
			}

			EndRebind(isOverriden);
		}

		/// <summary>
		/// Check ignore buttons.
		/// </summary>
		/// <param name="binding"></param>
		/// <returns></returns>
		bool IsIgnoreThrough(InputBinding binding)
		{
			string overridePath = GetTrueOverridePath(binding.overridePath);

			foreach (var str in ignoreThroughStrList)
			{
				if (string.Equals(str, overridePath)) return true;
			}
			return false;
		}

		/// <summary>
		/// Check cancel button.
		/// </summary>
		/// <param name="binding"></param>
		/// <returns></returns>
		bool IsCancellingThrough(InputBinding binding)
		{
			string overridePath = GetTrueOverridePath(binding.overridePath);

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

		/// <summary>
		/// Handle the swap binding.
		/// </summary>
		/// <param name="binding"></param>
		void HandleSwapBinding(InputBinding binding)
		{
			// Handle swapping.
			for (int i = 0; i < InputActionMap.bindings.Count; i++)
			{
				InputBinding bind = InputActionMap.bindings[i];
				if (bind.id == binding.id) continue;

				if ((string.IsNullOrEmpty(bind.overridePath) && bind.path.Equals(binding.effectivePath)) ||     // If still original bind and bind path == binding effective path or
					bind.effectivePath.Equals(binding.effectivePath))                                           // both the effective path are the same
				{
					Debug.Log("--------Swapped : " + bind.action + " Path : " + bind.path + " Eff Path : " + bind.effectivePath + " Over Path : " + bind.overridePath);
					InputActionRebindingExtensions.ApplyBindingOverride(InputActionMap, i, new InputBinding { overridePath = previousPath });
					break;
				}
			}
		}

		/// <summary>
		/// Get the override path with the type of controller.
		/// </summary>
		/// <param name="overridePath"></param>
		/// <returns></returns>
		string GetTrueOverridePath(string overridePath)
		{
			// If it's not part of keyboard or gamepad, assume it's a joystick.
			if (!overridePath.Contains("Keyboard") && !overridePath.Contains("Gamepad"))
			{
				string deviceStr = "<Gamepad>";
				if (!overridePath.Contains("SwitchProController")) deviceStr = "<Joystick>";

				overridePath = deviceStr + overridePath.Substring(overridePath.IndexOf('>') + 1);
			}
			return overridePath;
		}

		/// <summary>
		/// End the rebind session.
		/// </summary>
		/// <param name="isOverriden"></param>
		void EndRebind(bool isOverriden)
		{
			rebindingOperation.Dispose();

			InputDeviceType inputDeviceType = InputManager.Instance.InputDeviceType;
			InputDeviceType rebindDeviceType = currentSelection.InputDeviceTypeSet.InputDeviceType;

			if (isOverriden ||
				(rebindDeviceType == inputDeviceType) ||
				(rebindDeviceType == InputDeviceType.Gamepad && inputDeviceType == InputDeviceType.Joystick))
			{
				waitingForInputMenu.CloseWindow();
			}

			InputManager.Instance.EnableActionMap(ActionMapType.UI);
			UISelectable.LockSelection(false);

			OnRebinded?.Invoke();

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