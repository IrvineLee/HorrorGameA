using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
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

		ScrollRect scrollRect;
		CoroutineRun waitCR = new();

		protected override void Initialize()
		{
			uiSelectableList = GetComponentsInChildren<UISelectable>(true).ToList();
			scrollRect = GetComponentInChildren<ScrollRect>();
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

		public void SetCurrentIndex(int index) { currentActiveIndex = index; }

		void HandleMovement(Vector2 move)
		{
			if (move.y != 0)
			{
				currentActiveIndex = move.y > 0 ? currentActiveIndex - 1 : currentActiveIndex + 1;
				currentActiveIndex = currentActiveIndex.WithinCount(uiSelectableList.Count);

				EventSystem.current.SetSelectedGameObject(uiSelectableList[currentActiveIndex].gameObject);
				ScrollToSelected();
			}
			if (move.x != 0)
			{
				UISelectionBase currentSelection = uiSelectableList[currentActiveIndex].UISelectionBase;
				currentSelection.NextSelection(move.x > 0);
			}
		}

		void ScrollToSelected()
		{
			if (!scrollRect) return;
			if (uiSelectableList.Count <= 0) return;

			Vector2 nextNormalizesPosition = new Vector2(0, 1 - (currentActiveIndex / ((float)uiSelectableList.Count - 1)));

			Action<Vector2> callbackMethod = (result) => scrollRect.normalizedPosition = result;
			CoroutineHelper.LerpWithinSeconds(scrollRect.normalizedPosition, nextNormalizesPosition, ConstantFixed.UI_SCROLLBAR_DURATION, callbackMethod, isDeltaTime: false);
		}

		void OnDisable()
		{
			waitCR.StopCoroutine();
		}
	}
}
