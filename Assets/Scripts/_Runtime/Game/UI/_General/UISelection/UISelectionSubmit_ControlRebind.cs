using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using Personal.Manager;

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

			//InputAction = InputActionReference.Create(inputAct);
			Debug.Log("Test " + InputAction);
			Debug.Log("Test " + InputManager.Instance.PlayerActionInput.Player.Sprint);
		}

		public override void Submit()
		{
			Debug.Log("ADSD");
			controlRebind.StartRebind(this);
		}

		public override void Cancel()
		{
		}
	}
}
