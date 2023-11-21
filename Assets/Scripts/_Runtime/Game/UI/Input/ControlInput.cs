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

		protected virtual Vector2 GetHorizontalVerticalMovement(Vector2 move) { return Vector2.zero; }

		protected override void OnDisable()
		{
			base.OnDisable();
			waitCR.StopCoroutine();
		}
	}
}