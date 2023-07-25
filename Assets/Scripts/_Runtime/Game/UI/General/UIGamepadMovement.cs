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
		CoroutineRun waitCR = new CoroutineRun();

		bool isUpdate = true;

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
			if (!isUpdate) return;
			if (InputManager.Instance.IsCurrentDeviceMouse) return;
			if (InputManager.Instance.UIInputController.Move == Vector2.zero) return;
			if (!waitCR.IsDone) return;

			Vector2 move = InputManager.Instance.UIInputController.Move;
			HandleMovement(GetHorizontalOrVericalMovement(move));

			waitCR = CoroutineHelper.WaitFor(ConstantFixed.UI_SELECTION_DELAY, isRealSeconds: true);
		}

		public void SetCurrentIndex(int index) { currentActiveIndex = index; }
		public void SetIsUpdate(bool isFlag) { isUpdate = isFlag; }

		Vector2 GetHorizontalOrVericalMovement(Vector2 move)
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

			float ratio = 1 - (currentActiveIndex / ((float)uiSelectableList.Count - 1));
			Vector2 nextNormalizesPosition = new Vector2(0, ratio);

			Action<Vector2> callbackMethod = (result) => scrollRect.normalizedPosition = result;
			CoroutineHelper.LerpWithinSeconds(scrollRect.normalizedPosition, nextNormalizesPosition, ConstantFixed.UI_SCROLLBAR_DURATION, callbackMethod, isDeltaTime: false);
		}

		void OnDisable()
		{
			waitCR.StopCoroutine();
		}
	}
}
