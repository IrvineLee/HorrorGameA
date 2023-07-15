using UnityEngine;

using Personal.UI.Window;

namespace Personal.UI
{
	public abstract class UIHandlerBase : MenuUIBase
	{
		/// <summary>
		/// This is used for setting up the buttons and selection animation.
		/// </summary>
		public override void InitialSetup()
		{
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

			AdditionalSetup();
		}

		/// <summary>
		/// Additional setup for other things specific to their classes.
		/// </summary>
		protected virtual void AdditionalSetup() { }
	}
}
