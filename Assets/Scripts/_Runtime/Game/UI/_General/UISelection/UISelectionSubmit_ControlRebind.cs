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
		[SerializeField] InputActionReference inputAction = null;

		public InputAction InputAction { get; private set; }
		public TextMeshProUGUI NameTMP { get; private set; }
		public InputDeviceTypeSet InputDeviceTypeSet { get; private set; }
		public string IconInitials { get; private set; }

		ControlRebind controlRebind;

		public override void Initialize()
		{
			controlRebind = GetComponentInParent<ControlRebind>(true);
			NameTMP = GetComponentInChildren<TextMeshProUGUI>(true);
			InputDeviceTypeSet = GetComponentInParent<InputDeviceTypeSet>(true);

			// Get the 'player' action map and find the current action ex: Sprint.
			InputAction = InputManager.Instance.PlayerActionInput.asset.FindActionMap("Player").FindAction(inputAction.action.name);

			InputManager.OnDeviceIconChanged += IconChange;
			controlRebind.OnRemapped += IconChange;
		}

		void OnEnable()
		{
			IconChange();
		}

		public override void Submit()
		{
			controlRebind.StartRebind(this);
		}

		public override void Cancel()
		{
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
				string activeInputDeviceStr = inputDeviceType.ToString();
				bool condition01 = InputDeviceTypeSet.GetBindingGroupStr().Equals(activeInputDeviceStr) && bindDevice.Equals(activeInputDeviceStr);
				bool condition02 = (!bindDevice.Equals("Keyboard") && !bindDevice.Equals("Gamepad") && inputDeviceType == InputDeviceType.Joystick);

				if (condition01 || condition02)
				{
					displayStr = GetDisplayString(binding, inputDeviceType);
					break;
				}
			}

			IconInitials = InputManager.Instance.IconInitials;
			if (InputDeviceTypeSet.InputDeviceType == InputDeviceType.Gamepad) IconInitials = InputManager.Instance.GamepadIconInitials;

			NameTMP.text = string.IsNullOrEmpty(displayStr) ? NameTMP.text : (IconInitials + displayStr).SpriteEnclose();
			Debug.Log("displayStr " + (IconInitials + displayStr) + "    " + NameTMP.text);
		}

		string GetDisplayString(InputBinding binding, InputDeviceType inputDeviceType)
		{
			// You might want to add more restrictions here for other possible naming.
			string displayStr = binding.ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);

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

		void OnDestroy()
		{
			InputManager.OnDeviceIconChanged -= IconChange;
			controlRebind.OnRemapped -= IconChange;
		}
	}
}