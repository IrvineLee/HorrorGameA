using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Personal.Manager;
using Personal.GameState;

namespace Personal.UI
{
	public class ControlRebind : GameInitialize
	{
		[SerializeField] GameObject waitingForInputGO = null;
		[SerializeField] List<string> cancelThroughStrList = null;

		InputActionRebindingExtensions.RebindingOperation rebindingOperation;

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

			overridePathList.Clear();
			for (int i = 0; i < inputAction.controls.Count; i++)
			{
				int bindingIndex = inputAction.GetBindingIndexForControl(inputAction.controls[i]);
				string overridePath = inputAction.bindings[bindingIndex].overridePath;

				overridePathList.Add(overridePath);
			}

			rebindingOperation = inputAction.PerformInteractiveRebinding()
				.WithControlsExcluding("Mouse")
				.OnMatchWaitForAnother(0.1f)
				.WithCancelingThrough("<Keyboard>/escape")
				.OnCancel((operation) => EndRebind())
				.OnComplete((operation) => RebindComplete())
				.Start();

		}

		void RebindComplete()
		{
			int bindingIndex = inputAction.GetBindingIndexForControl(inputAction.controls[0]);
			var binding = inputAction.bindings[bindingIndex];
			var humanReadableType = InputControlPath.HumanReadableStringOptions.OmitDevice;

			Debug.Log("binding.overridePath " + binding.overridePath);
			if (IsCancellingThrough(binding)) return;

			currentSelection.NameTMP.text = InputControlPath.ToHumanReadableString(binding.effectivePath, humanReadableType);
			EndRebind();

			Debug.Log("REBIND Complete!");
		}

		bool IsCancellingThrough(InputBinding binding)
		{
			bool isCancel = false;
			foreach (var str in cancelThroughStrList)
			{
				if (!string.Equals(str, binding.overridePath)) continue;

				isCancel = true;
				break;
			}

			if (isCancel)
			{
				for (int i = 0; i < overridePathList.Count; i++)
				{
					InputActionRebindingExtensions.ApplyBindingOverride(inputAction, i, new InputBinding { overridePath = overridePathList[i] });
				}
			}

			foreach (var b in inputAction.bindings)
			{
				Debug.Log("Binding  " + b);
			}

			EndRebind();
			return isCancel;
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