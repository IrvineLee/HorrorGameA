using System;
using UnityEngine;

using Personal.Manager;
using Personal.GameState;
using Helper;
using Personal.Constant;
using static Personal.Manager.InputManager;

namespace Personal.UI
{
	/// <summary>
	/// Abstract class for gamepad movement.
	/// </summary>
	public abstract class GamepadMovement : GameInitialize
	{
		public static bool IsHold { get; private set; }

		public int CurrentActiveIndex { get => currentActiveIndex; }

		protected int currentActiveIndex;

		CoroutineRun waitCR = new CoroutineRun();

		protected virtual void OnEnable()
		{
			currentActiveIndex = 0;
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
		/// Update the current selection.
		/// </summary>
		/// <param name="go"></param>
		public virtual void UpdateCurrentSelection(GameObject go) { }

		protected virtual void HandleMovement(Vector2 move) { }

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

		protected virtual void OnDisable()
		{
			waitCR.StopCoroutine();
		}
	}
}
