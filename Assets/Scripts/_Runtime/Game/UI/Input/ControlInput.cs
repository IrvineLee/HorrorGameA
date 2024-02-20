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

		public static bool IsHold { get; private set; }

		public int CurrentActiveIndex { get; protected set; }

		CoroutineRun waitCR = new CoroutineRun();

		void Update()
		{
			HandleConfirmPressed();
			HandleMotionPressed();
		}

		/// <summary>
		/// Update the current selection.
		/// </summary>
		/// <param name="go"></param>
		public virtual void UpdateCurrentSelection(GameObject go) { }

		protected virtual void HandleConfirmPressed()
		{
			if (!isConfirmPressedMoveToNext) return;

			bool isConfirmbuttonPressed = InputManager.Instance.GetButtonPush(ButtonPush.Submit);
			if (!isConfirmbuttonPressed) return;

			HandleMovement(GetHorizontalVerticalMovement(Vector2.right), HandleEndConfirmButton);
		}

		protected virtual void HandleMotionPressed()
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

		protected virtual void HandleMovement(Vector2 move, Action endConfirmButtonAction = default) { }
		protected virtual void HandleEndConfirmButton() { }

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

		protected override void OnDisabled()
		{
			base.OnDisabled();
			waitCR.StopCoroutine();
		}
	}
}