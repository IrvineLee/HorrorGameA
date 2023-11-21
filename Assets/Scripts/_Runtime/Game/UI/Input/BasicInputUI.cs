using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Helper;

namespace Personal.UI
{
	/// <summary>
	/// Use this for UI input controls. This should handle the movement of selections.
	/// </summary>
	public class BasicInputUI : ControlInput
	{
		public int CurrentActiveIndex { get; private set; }
		public AutoScrollRect AutoScrollRect { get; private set; }

		List<UISelectable> uiSelectableList = new();

		/// <summary>
		/// Set the ui values for the currently opened UI's.
		/// </summary>
		/// <param name="uiSelectableList"></param>
		/// <param name="autoScrollRect"></param>
		public void SetUIValues(List<UISelectable> uiSelectableList, AutoScrollRect autoScrollRect)
		{
			CurrentActiveIndex = 0;

			this.uiSelectableList = uiSelectableList;
			AutoScrollRect = autoScrollRect;

			AutoScrollRect?.SetSelectionToTop();
		}

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
				AutoScrollRect?.ScrollToSelected();
			}
			if (move.x != 0)
			{
				UISelectionBase currentSelection = uiSelectableList[CurrentActiveIndex].UISelectionBase;
				currentSelection?.NextSelection(move.x > 0);
			}
		}

		protected override Vector2 GetHorizontalVerticalMovement(Vector2 move)
		{
			if (MathF.Abs(move.x) > MathF.Abs(move.y))
			{
				move.x = move.x > 0 ? 1 : -1;
				move.y = 0;
			}
			else
			{
				move.x = 0;
				move.y = move.y > 0 ? 1 : -1;
			}
			return move;
		}
	}
}