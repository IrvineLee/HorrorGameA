using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using Helper;

namespace Personal.UI
{
	/// <summary>
	/// This handles the gamepad movement for UI.
	/// </summary>
	public class UIGamepadMovement : GamepadMovement
	{
		public AutoScrollRect AutoScrollRect { get; private set; }

		List<UISelectable> uiSelectableList = new();

		protected override void Initialize()
		{
			uiSelectableList = GetComponentsInChildren<UISelectable>().ToList();
			AutoScrollRect = GetComponentInChildren<AutoScrollRect>();
		}

		/// <summary>
		/// Call this when some new ui gets initialized and selectables need to be refresh.
		/// </summary>
		public void RefreshCacheValues() { Initialize(); }

		/// <summary>
		/// Update the current selection.
		/// </summary>
		/// <param name="go"></param>
		public override void UpdateCurrentSelection(GameObject go)
		{
			for (int i = 0; i < uiSelectableList.Count; i++)
			{
				GameObject uiSelectableGO = uiSelectableList[i].gameObject;

				if (!uiSelectableGO.Equals(go)) continue;

				currentActiveIndex = i;
				break;
			}
		}

		protected override void HandleMovement(Vector2 move)
		{
			if (uiSelectableList.Count <= 0) return;

			if (move.y != 0)
			{
				currentActiveIndex = move.y > 0 ? currentActiveIndex - 1 : currentActiveIndex + 1;
				currentActiveIndex = currentActiveIndex.WithinCount(uiSelectableList.Count);

				EventSystem.current.SetSelectedGameObject(uiSelectableList[currentActiveIndex].gameObject);
				AutoScrollRect?.ScrollToSelected();
			}
			if (move.x != 0)
			{
				UISelectionBase currentSelection = uiSelectableList[currentActiveIndex].UISelectionBase;
				currentSelection?.NextSelection(move.x > 0);
			}
		}
	}
}
