using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Helper;
using Personal.UI.Window;
using Personal.InputProcessing;

namespace Personal.UI
{
	public abstract class UIHandlerBase : MenuUI
	{
		[SerializeField] bool isHandleSelectables = true;

		protected List<UISelectable> uiSelectableList = new();
		protected AutoScrollRect autoScrollRect;

		/// <summary>
		/// This is used for setting up the buttons and selection animation.
		/// </summary>
		public override void InitialSetup()
		{
			base.InitialSetup();

			Transform goParent = windowUIAnimator != null ? windowUIAnimator.transform : transform;

			var buttonInteractArray = goParent.GetComponentsInChildren<ButtonInteractBase>(true);
			foreach (var buttonInteract in buttonInteractArray)
			{
				buttonInteract.InitialSetup();
			}

			var selectionAnimatorArray = goParent.GetComponentsInChildren<WindowSelectionUIAnimator>(true);
			foreach (var selection in selectionAnimatorArray)
			{
				selection.InitialSetup();
			}

			var selectionArray = goParent.GetComponentsInChildren<UISelectionBase>(true);
			foreach (var selection in selectionArray)
			{
				selection.InitialSetup();
			}

			if (!isHandleSelectables) return;

			uiSelectableList = GetComponentsInChildren<UISelectable>(true).ToList();
			autoScrollRect = GetComponentInChildren<AutoScrollRect>(true);
		}

		protected override void OnEnabled()
		{
			if (!isHandleSelectables) return;
			if (!ControlInputBase.ActiveControlInput) return;
			if (ControlInputBase.ActiveControlInput.GetType() == typeof(InputMovement_FPSController)) return;

			((BasicControllerUI)ControlInputBase.ActiveControlInput)?.SetUIValues(uiSelectableList, autoScrollRect);
		}

	}
}
