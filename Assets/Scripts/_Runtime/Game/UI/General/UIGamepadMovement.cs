using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using Personal.Manager;
using Personal.GameState;
using Helper;
using Personal.Constant;

namespace Personal.UI
{
	public class UIGamepadMovement : GameInitialize
	{
		List<UISelectable> uiSelectableList = new();
		int currentActiveIndex;

		CoroutineRun waitCR = new();

		protected override void Initialize()
		{
			uiSelectableList = GetComponentsInChildren<UISelectable>(true).ToList();
		}

		void OnEnable()
		{
			currentActiveIndex = 0;
		}

		void Update()
		{
			if (InputManager.Instance.IsCurrentDeviceMouse) return;
			if (InputManager.Instance.UIInputController.MoveOnce == Vector2.zero && !waitCR.IsDone) return;

			waitCR.StopCoroutine();

			Vector2 move = InputManager.Instance.UIInputController.Move;
			bool isHold = move != Vector2.zero;

			if (isHold)
			{
				waitCR = CoroutineHelper.WaitFor(ConstantFixed.UI_SELECTION_DELAY);
			}
			else
			{
				move = InputManager.Instance.UIInputController.MoveOnce;
			}

			HandleMovement(move);
		}

		void HandleMovement(Vector2 move)
		{
			if (move.y != 0)
			{
				currentActiveIndex = move.y > 0 ? currentActiveIndex - 1 : currentActiveIndex + 1;
				currentActiveIndex = currentActiveIndex.WithinCount(uiSelectableList.Count);

				EventSystem.current.SetSelectedGameObject(uiSelectableList[currentActiveIndex].gameObject);
			}
			if (move.x != 0)
			{
				SelectionListing currentSelection = uiSelectableList[currentActiveIndex].SelectionListing;
				currentSelection.NextSelection(move.x > 0);
			}
		}

		void OnDisable()
		{
			waitCR.StopCoroutine();
		}
	}
}
