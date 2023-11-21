using System;
using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.InputProcessing;
using Personal.Constant;
using static Personal.Manager.InputManager;

namespace Personal.UI
{
	public abstract class ControlInput : ControlInputBase
	{
		public static bool IsHold { get; private set; }

		CoroutineRun waitCR = new CoroutineRun();

		void Update()
		{
			Vector3 move = InputManager.Instance.GetMotion(MotionType.Move);

			if (move == Vector3.zero)
			{
				IsHold = false;
				return;
			}
			if (!waitCR.IsDone) return;

			HandleMovement(GetHorizontalVerticalMovement(move));
			IsHold = true;

			waitCR = CoroutineHelper.WaitFor(ConstantFixed.UI_SELECTION_DELAY, isRealSeconds: true);
		}

		/// <summary>
		/// Update the current selection.
		/// </summary>
		/// <param name="go"></param>
		public virtual void UpdateCurrentSelection(GameObject go) { }

		protected virtual void HandleMovement(Vector2 move) { }

		/// <summary>
		/// Handle the analog input so it gives concrete value for a smoother experience. DPad has no problem.
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		protected virtual Vector2 GetHorizontalVerticalMovement(Vector2 move)
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

		protected override void OnDisable()
		{
			base.OnDisable();
			waitCR.StopCoroutine();
		}
	}
}