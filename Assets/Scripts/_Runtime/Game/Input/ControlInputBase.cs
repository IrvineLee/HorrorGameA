using UnityEngine;

using Personal.GameState;

namespace Personal.InputProcessing
{
	public abstract class ControlInputBase : GameInitialize
	{
		public static ControlInputBase ActiveControlInput { get; protected set; }

		static ControlInputBase previousActiveControl;

		protected override void OnEnabled()
		{
			ActiveControlInput = this;
		}

		protected override void OnDisabled()
		{
			ActiveControlInput = null;
		}

		public static void DisableSaveActiveControl()
		{
			previousActiveControl = ActiveControlInput;
			ActiveControlInput = null;
		}

		public static void EnableLoadActiveControl()
		{
			if (!previousActiveControl)
			{
				Debug.Log("No Previous Action Control");
				return;
			}

			ActiveControlInput = previousActiveControl;
			previousActiveControl = null;
		}
	}
}