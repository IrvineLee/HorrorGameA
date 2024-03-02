using System;
using UnityEngine;

using Helper;
using Personal.Manager;
using Personal.Constant;
using static Personal.Manager.InputManager;

namespace Personal.InputProcessing
{
	public abstract class ControlInput : ControlInputBase
	{
		[Tooltip("Whether pressing the confirm button move it to the next selection.")]
		[SerializeField] bool isConfirmPressedMoveToNext = false;

		[Tooltip("Whether pressing the cancel button move it to the previous selection.")]
		[SerializeField] bool isCancelPressedMoveToPrevious = false;

		public static bool IsHold { get; private set; }

		public int CurrentActiveIndex { get; protected set; }

		CoroutineRun waitCR = new CoroutineRun();

		void Update()
		{
			HandleConfirmPressed();
			HandleCancelPressed();
			HandleMotionPressed();
		}

		/// <summary>
		/// Update the current selection.
		/// </summary>
		/// <param name="go"></param>
		public virtual void UpdateCurrentSelection(GameObject go) { }

		protected virtual void HandleMovement(Vector2 move, Action endConfirmButtonAction = default) { }
		protected virtual void HandleEndConfirmButton() { }

		protected void HandleConfirmPressed()
		{
			if (!isConfirmPressedMoveToNext) return;
			if (!InputManager.Instance.GetButtonPush(ButtonPush.Submit)) return;

			HandleMovement(GetHorizontalVerticalMovement(GetSubmitPressedMoveDirection()), HandleEndConfirmButton);
		}

		protected void HandleCancelPressed()
		{
			if (!isCancelPressedMoveToPrevious) return;
			if (!InputManager.Instance.GetButtonPush(ButtonPush.Cancel)) return;

			HandleMovement(GetHorizontalVerticalMovement(GetCancelPressedMoveDirection()));
		}

		/// <summary>
		/// Handle the D-pad and left analog movement.
		/// </summary>
		protected void HandleMotionPressed()
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
		/// Handle the analog input so it gives concrete value for a smoother experience. D-pad has no problem.
		/// For movement in menu ui selection.
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

		protected virtual Vector2 GetSubmitPressedMoveDirection() { return Vector2.right; }
		protected virtual Vector2 GetCancelPressedMoveDirection() { return Vector2.left; }

		protected override void OnDisabled()
		{
			base.OnDisabled();
			waitCR.StopCoroutine();
		}
	}
}