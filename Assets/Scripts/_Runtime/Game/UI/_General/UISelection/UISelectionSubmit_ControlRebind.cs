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

		ControlRebind controlRebind;

		public override void Initialize()
		{
			controlRebind = GetComponentInParent<ControlRebind>(true);
			NameTMP = GetComponentInChildren<TextMeshProUGUI>(true);

			// Get the 'player' action map and find the current action ex: Sprint.
			InputAction = InputManager.Instance.PlayerActionInput.asset.FindActionMap("Player").FindAction(inputAction.action.name);
			InputManager.OnDeviceIconChanged += IconChange;
		}

		public override void Submit()
		{
			controlRebind.StartRebind(this);
		}

		public override void Cancel()
		{
		}

		void IconChange()
		{
			string displayStr = "";
			foreach (var binding in InputAction.bindings)
			{
				// You only want the value within the square braces.
				string device = binding.ToDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice).SearchBehindRemoveFrontOrEnd('[', true, false);
				device = device.SearchBehindRemoveFrontOrEnd(']', true);

				Debug.Log(binding);
				if (!device.Equals("Gamepad")) continue;

				displayStr = binding.ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
				break;
			}

			// You might want to add more restrictions here for other possible naming.
			displayStr = displayStr.Contains("Button") ? displayStr.Replace(" ", "_") : displayStr.RemoveAllWhiteSpaces();
			NameTMP.text = (InputManager.Instance.IconInitials + displayStr).SpriteEnclose();
		}

		void OnDestroy()
		{
			InputManager.OnDeviceIconChanged -= IconChange;
		}
	}
}