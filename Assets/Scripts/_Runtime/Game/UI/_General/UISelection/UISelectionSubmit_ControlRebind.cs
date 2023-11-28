using System;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI
{
	public class UISelectionSubmit_ControlRebind : UISelectionBase
	{
		[SerializeField] InputActionReference inputAction = null;

		public InputAction InputAction { get; private set; }
		public int DeviceIndex { get; private set; }
		public string BindingGroup { get; private set; }
		public TextMeshProUGUI NameTMP { get; private set; }

		ControlRebind controlRebind;
		InputDeviceTypeSet inputDeviceTypeSet;

		public override void Initialize()
		{
			controlRebind = GetComponentInParent<ControlRebind>(true);
			inputDeviceTypeSet = GetComponentInParent<InputDeviceTypeSet>(true);
			NameTMP = GetComponentInChildren<TextMeshProUGUI>(true);

			// Get the 'player' action map and find the current action ex: Sprint.
			InputAction = InputManager.Instance.PlayerActionInput.asset.FindActionMap("Player").FindAction(inputAction.action.name);

			foreach (var binding in InputAction.bindings)
			{
				//var control = InputControlPath.TryFindControl(device, binding.effectivePath);
				Debug.Log("Binding " + binding);
			}

			// Get the input device.
			BindingGroup = inputDeviceTypeSet.GetBindingGroup();
			string displayStr = InputAction.GetBindingDisplayString(InputBinding.MaskByGroup(BindingGroup));

			NameTMP.text = displayStr;
		}

		public override void Submit()
		{
			controlRebind.StartRebind(this);
		}

		public override void Cancel()
		{
		}

		string GenerateHelpText(InputAction action)
		{
			if (action.controls.Count == 0)
				return string.Empty;

			var verb = action.type == InputActionType.Button ? "Press" : "Use";
			var lastCompositeIndex = -1;
			var isFirstControl = true;

			var controls = "";
			foreach (var control in action.controls)
			{
				var bindingIndex = action.GetBindingIndexForControl(control);
				var binding = action.bindings[bindingIndex];
				if (binding.isPartOfComposite)
				{
					if (lastCompositeIndex != -1)
						continue;
					lastCompositeIndex = action.ChangeBinding(bindingIndex).PreviousCompositeBinding().bindingIndex;
					bindingIndex = lastCompositeIndex;
				}
				else
				{
					lastCompositeIndex = -1;
				}
				if (!isFirstControl)
					controls += " or ";

				controls += action.GetBindingDisplayString(bindingIndex);
				isFirstControl = false;
			}
			return $"{verb} {controls} to {action.name.ToLower()}";
		}

	}
}
