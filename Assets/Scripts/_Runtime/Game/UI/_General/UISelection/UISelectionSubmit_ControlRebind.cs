using System;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using Helper;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.UI
{
	public class UISelectionSubmit_ControlRebind : UISelectionBase
	{
		[SerializeField] InputActionReference inputActionReference = null;

		public InputAction InputAction { get; private set; }
		public InputBinding InputBinding { get; private set; }
		public TextMeshProUGUI NameTMP { get; private set; }
		public InputDeviceTypeSet InputDeviceTypeSet { get; private set; }          // InputDeviceType here can only be Keyboard/Gamepad only. Joystick is treated as Gamepad.
		public string IconInitials { get; private set; }

		protected ControlRebind controlRebind;

		public override void Initialize()
		{
			controlRebind = GetComponentInParent<ControlRebind>(true);
			NameTMP = GetComponentInChildren<TextMeshProUGUI>(true);
			InputDeviceTypeSet = GetComponentInParent<InputDeviceTypeSet>(true);

			// Get the 'player' action map and find the current action ex: Sprint.
			InputAction = controlRebind.InputActionMap.FindAction(inputActionReference.action.name);

			InputManager.OnDeviceIconChanged += IconChange;
			controlRebind.OnRebinded += IconChange;
		}

		void OnEnable()
		{
			IconChange();
		}

		public override void Submit()
		{
			controlRebind.StartRebind(this);
		}

		public void RefreshUI()
		{
			IconChange();
			//Debug.Log("Refresh UI");
		}

		/// <summary>
		/// Only handle the icon change.
		/// </summary>
		void IconChange()
		{
			if (!gameObject.activeInHierarchy) return;

			string displayStr = "";
			InputDeviceType inputDeviceType = InputManager.Instance.InputDeviceType;

			foreach (var binding in InputAction.bindings)
			{
				// You only want the value within the square braces. [Keyboard][Gamepad][Joystick]
				string bindDevice = binding.ToDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice).SearchBehindRemoveFrontOrEnd('[', true, false);
				bindDevice = bindDevice.SearchBehindRemoveFrontOrEnd(']', true);

				// Get the correct binding.
				string currentInputDeviceStr = InputDeviceTypeSet.GetBindingGroupStr();

				bool condition01 = inputDeviceType != InputDeviceType.Joystick && bindDevice.Equals(currentInputDeviceStr);
				bool condition02 = (!bindDevice.Equals("Keyboard") && !bindDevice.Equals("Gamepad") && inputDeviceType == InputDeviceType.Joystick);

				if (!condition01 && !condition02) continue;

				if (condition02) inputDeviceType = InputDeviceType.Joystick;

				InputBinding = binding;
				displayStr = GetDisplayString(binding, inputDeviceType);
				break;
			}

			IconInitials = InputManager.Instance.IconInitials;
			if (InputDeviceTypeSet.InputDeviceType == InputDeviceType.Gamepad) IconInitials = InputManager.Instance.GamepadIconInitials;

			NameTMP.text = string.IsNullOrEmpty(displayStr) ? NameTMP.text : (IconInitials + displayStr).SpriteEnclose();
			//Debug.Log("displayStr " + (IconInitials + displayStr) + "    " + NameTMP.text);
		}

		string GetDisplayString(InputBinding binding, InputDeviceType inputDeviceType)
		{
			// You might want to add more restrictions here for other possible naming.
			var displayStringOption = InputBinding.DisplayStringOptions.DontUseShortDisplayNames;
			string displayStr = !binding.isComposite ? binding.ToDisplayString(displayStringOption) : HandleCompositeBinding(InputAction);

			bool isKeyboardOrGamepad = (inputDeviceType != InputDeviceType.Joystick && displayStr.Contains("Button"));
			displayStr = isKeyboardOrGamepad ? displayStr.Replace(" ", "_") : displayStr.RemoveAllWhiteSpaces();

			// If it's joystick, try parse it to button icon type.
			if (inputDeviceType == InputDeviceType.Joystick &&
				Enum.TryParse(displayStr, true, out JoystickToGenericButtonIcon parsedIcon))
			{
				displayStr = parsedIcon.GetStringValue();
			}
			return displayStr;
		}

		protected virtual string HandleCompositeBinding(InputAction inputAction) { return ""; }

		void OnDestroy()
		{
			InputManager.OnDeviceIconChanged -= IconChange;
			controlRebind.OnRebinded -= IconChange;
		}
	}
}