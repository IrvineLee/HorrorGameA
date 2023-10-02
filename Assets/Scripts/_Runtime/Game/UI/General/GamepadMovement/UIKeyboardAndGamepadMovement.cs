using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Personal.Manager;
using Personal.GameState;
using Helper;
using Personal.Constant;
using static Personal.Manager.InputManager;

namespace Personal.UI
{
	public class UIKeyboardAndGamepadMovement : GameInitialize
	{
		public int CurrentActiveIndex { get => currentActiveIndex; }
		public static bool IsHold { get; private set; }

		List<UISelectable> uiSelectableList = new();

		AutoScrollRect autoScrollRect;
		CoroutineRun waitCR = new CoroutineRun();

		protected int currentActiveIndex;

		protected override void Initialize()
		{
			uiSelectableList = GetComponentsInChildren<UISelectable>(true).ToList();
			autoScrollRect = GetComponentInChildren<AutoScrollRect>();
		}

		protected virtual void OnEnable()
		{
			currentActiveIndex = 0;
			InputManager.OnDeviceIconChanged += OnDeviceChanged;
		}

		void Update()
		{
			Vector3 move = InputManager.Instance.GetMotion(MotionType.Move);

			if (move == Vector3.zero)
			{
				IsHold = false;
				return;
			}
			if (!waitCR.IsDone) return;

			HandleMovement(GetHorizontalVericalMovement(move));
			IsHold = true;

			waitCR = CoroutineHelper.WaitFor(ConstantFixed.UI_SELECTION_DELAY, isRealSeconds: true);
		}

		public void SetCurrentIndex(int index) { currentActiveIndex = index; }

		protected virtual void HandleMovement(Vector2 move)
		{
			if (move.y != 0)
			{
				currentActiveIndex = move.y > 0 ? currentActiveIndex - 1 : currentActiveIndex + 1;
				currentActiveIndex = currentActiveIndex.WithinCount(uiSelectableList.Count);

				EventSystem.current.SetSelectedGameObject(uiSelectableList[currentActiveIndex].gameObject);
				autoScrollRect?.ScrollToSelected();
			}
			if (move.x != 0)
			{
				UISelectionBase currentSelection = uiSelectableList[currentActiveIndex].UISelectionBase;
				currentSelection.NextSelection(move.x > 0);
			}
		}

		protected virtual Vector2 GetHorizontalVericalMovement(Vector2 move)
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

		protected virtual void OnDeviceChanged() { }

		protected virtual void OnDisable()
		{
			InputManager.OnDeviceIconChanged -= OnDeviceChanged;
			waitCR.StopCoroutine();
		}
	}
}
