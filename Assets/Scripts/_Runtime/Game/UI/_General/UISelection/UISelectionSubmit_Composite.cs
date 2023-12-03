using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Personal.UI
{
	public class UISelectionSubmit_Composite : UISelectionSubmit_ControlRebind
	{
		[Tooltip("The binding index within the composite.")]
		[SerializeField] int index = 0;

		public override void Submit()
		{
			if (!IsSubmittable()) return;
			controlRebind.StartRebind(this, index);
		}

		protected override string HandleCompositeBinding(InputAction inputAction)
		{
			var displayStringOption = InputBinding.DisplayStringOptions.DontUseShortDisplayNames;
			return inputAction.bindings[index].ToDisplayString(displayStringOption);
		}
	}
}