using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

using Personal.Manager;
using Personal.GameState;

namespace Personal.UI
{
	public class ControlRebind : GameInitialize
	{
		[SerializeField] GameObject waitingForInputGO = null;
		[SerializeField] List<string> cancelThroughStrList = null;

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

			Debug.Log("REBINDING! " + currentSelection + "     " + currentSelection.InputAction);

			waitingForInputGO.SetActive(true);
			InputManager.Instance.DisableAllActionMap();

			// Stash the overridePath to reset it in case of cancelling through with different keys.
			overridePathList.Clear();
			foreach (var binding in inputAction.bindings)
			{
				overridePathList.Add(binding.overridePath);
			}

			rebindingOperation = inputAction.PerformInteractiveRebinding()
				.WithBindingGroup(currentSelection.BindingGroup)
				.WithControlsExcluding("Mouse")
				.WithControlsHavingToMatchPath("<" + currentSelection.BindingGroup + ">")
				.OnMatchWaitForAnother(0.1f)
				.WithCancelingThrough("<Keyboard>/escape")
				.OnCancel((operation) => EndRebind())
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
			if (!IsCancellingThrough(binding))
			{
				currentSelection.NameTMP.text = InputControlPath.ToHumanReadableString(binding.effectivePath, humanReadableType);
				Debug.Log("Override Complete!");
			}

			foreach (var b in inputAction.bindings)
			{
				Debug.Log("Binding  " + b);
			}

			EndRebind();
		}

		bool IsCancellingThrough(InputBinding binding)
		{
			// If it's not part of keyboard or gamepad, assume it's a joystick.
			string overridePath = binding.overridePath;
			if (!overridePath.Contains("Keyboard") || !overridePath.Contains("Gamepad"))
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

		void EndRebind()
		{
			rebindingOperation.Dispose();
			waitingForInputGO.SetActive(false);

			InputManager.Instance.EnableActionMap(InputProcessing.ActionMapType.UI);

			Debug.Log("REBIND End!");
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