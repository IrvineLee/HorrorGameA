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
	/// <summary>
	/// This handles the gamepad movement for UI.
	/// </summary>
	public class UIGamepadMovement : GameInitialize
	{
		public int CurrentActiveIndex { get => currentActiveIndex; }
		public AutoScrollRect AutoScrollRect { get; private set; }
		public static bool IsHold { get; private set; }

		protected int currentActiveIndex;

		List<UISelectable> uiSelectableList = new();
		CoroutineRun waitCR = new CoroutineRun();

		protected override void Initialize()
		{
			uiSelectableList = GetComponentsInChildren<UISelectable>().ToList();
			AutoScrollRect = GetComponentInChildren<AutoScrollRect>();
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

		/// <summary>
		/// Call this when some new ui gets initialized and selectables need to be refresh.
		/// </summary>
		public void RefreshCacheValues() { Initialize(); }

		/// <summary>
		/// Update the current selection.
		/// </summary>
		/// <param name="go"></param>
		public void UpdateCurrentSelection(GameObject go)
		{
			for (int i = 0; i < uiSelectableList.Count; i++)
			{
				GameObject uiSelectableGO = uiSelectableList[i].gameObject;

				if (uiSelectableGO != go) continue;

				currentActiveIndex = i;
				break;
			}
		}

		protected virtual void HandleMovement(Vector2 move)
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
