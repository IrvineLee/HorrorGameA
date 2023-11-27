using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using Personal.Manager;
using UnityEngine.InputSystem.Utilities;

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

			InputAction = InputManager.Instance.PlayerActionInput.asset.FindActionMap("Player").FindAction(inputAction.action.name);

			int bindingIndex = InputAction.GetBindingIndexForControl(InputAction.controls[0]);
			string effectivePath = InputAction.bindings[bindingIndex].effectivePath;
			var humanReadableOption = InputControlPath.HumanReadableStringOptions.OmitDevice;

			NameTMP.text = InputControlPath.ToHumanReadableString(effectivePath, humanReadableOption);

			foreach (var control in InputAction.controls)
			{
				Debug.Log("Control " + control);
			}

			////InputAction = InputActionReference.Create(inputAct);
			//foreach (var binding in InputAction.bindings)
			//{
			//	Debug.Log("Binding  " + binding);
			//}
			//Debug.Log("Test " + InputManager.Instance.PlayerActionInput.Player.Sprint);
		}

		public override void Submit()
		{
			controlRebind.StartRebind(this);
		}

		public override void Cancel()
		{
		}
	}
}
