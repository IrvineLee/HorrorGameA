using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Helper;
using Personal.UI;

namespace Personal.InputProcessing
{
	/// <summary>
	/// Use this for UI input controls. This should handle the movement of selections.
	/// </summary>
	public class BasicControllerUI : ControlInput
	{
		protected List<UISelectable> uiSelectableList = new();
		protected AutoScrollRect autoScrollRect;

		/// <summary>
		/// Set the ui values for the currently opened UI's.
		/// </summary>
		/// <param name="uiSelectableList"></param>
		/// <param name="autoScrollRect"></param>
		public void SetUIValues(List<UISelectable> uiSelectableList, AutoScrollRect autoScrollRect)
		{
			CurrentActiveIndex = 0;

			this.uiSelectableList = uiSelectableList;
			this.autoScrollRect = autoScrollRect;

			this.autoScrollRect?.SetSelectionToTop();
		}

		/// <summary>
		/// Update the current selection. Set the active index in the selectableList based on the gameobject.
		/// </summary>
		/// <param name="go"></param>
		public override void UpdateCurrentSelection(GameObject go)
		{
			for (int i = 0; i < uiSelectableList.Count; i++)
			{
				GameObject uiSelectableGO = uiSelectableList[i].gameObject;

				if (!uiSelectableGO.Equals(go)) continue;

				CurrentActiveIndex = i;
				break;
			}
		}

		protected override void HandleMovement(Vector2 move)
		{
			if (uiSelectableList.Count <= 0) return;

			if (move.y != 0)
			{
				CurrentActiveIndex = move.y > 0 ? CurrentActiveIndex - 1 : CurrentActiveIndex + 1;
				CurrentActiveIndex = CurrentActiveIndex.WithinCount(uiSelectableList.Count);

				EventSystem.current.SetSelectedGameObject(uiSelectableList[CurrentActiveIndex].gameObject);
				autoScrollRect?.ScrollToSelected();
			}
			if (move.x != 0)
			{
				UISelectionBase currentSelection = uiSelectableList[CurrentActiveIndex].UISelectionBase;
				currentSelection?.NextSelection(move.x > 0);
			}
		}
	}
}