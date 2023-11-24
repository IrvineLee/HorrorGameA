using UnityEngine;
using UnityEngine.InputSystem;

using Personal.Manager;

namespace Personal.UI
{
	public class ControlRebind : MonoBehaviour
	{
		[SerializeField] GameObject waitingForInputGO = null;

		InputActionRebindingExtensions.RebindingOperation rebindingOperation;
		UISelectionSubmit_ControlRebind currentSelection;

		public void StartRebind(UISelectionSubmit_ControlRebind uiSelectionSubmit)
		{
			Debug.Log("REBINDING!");
			currentSelection = uiSelectionSubmit;

			waitingForInputGO.SetActive(true);
			InputManager.Instance.DisableAllActionMap();

			rebindingOperation = currentSelection.InputAction.PerformInteractiveRebinding()
				.WithControlsExcluding("Mouse")
				.OnMatchWaitForAnother(0.1f)
				.WithCancelingThrough("<Keyboard>/escape")
				//.WithCancelingThrough("<Gamepad>/start")
				.OnCancel((operation) => EndRebind())
				.OnComplete((operation) => RebindComplete())
				.Start();
		}

		void RebindComplete()
		{
			InputAction inputAction = currentSelection.InputAction;
			int bindingIndex = inputAction.GetBindingIndexForControl(inputAction.controls[0]);

			currentSelection.NameTMP.text = InputControlPath.ToHumanReadableString(inputAction.bindings[bindingIndex].effectivePath,
				InputControlPath.HumanReadableStringOptions.OmitDevice);

			Debug.Log("REBIND Complete!");
			EndRebind();
		}

		void EndRebind()
		{
			rebindingOperation.Dispose();
			waitingForInputGO.SetActive(false);

			InputManager.Instance.EnableActionMap(InputProcessing.ActionMapType.UI);

			Debug.Log("REBIND End!");
		}
	}
}