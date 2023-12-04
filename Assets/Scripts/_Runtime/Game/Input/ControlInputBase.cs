using UnityEngine;

using Personal.GameState;

namespace Personal.InputProcessing
{
	public abstract class ControlInputBase : GameInitialize
	{
		public static ControlInputBase ActiveControlInput { get; protected set; }

		protected static bool isStop;

		public static void StopControl(bool isFlag) { isStop = isFlag; }

		protected override void OnEnabled()
		{
			ActiveControlInput = this;
		}

		protected override void OnDisabled()
		{
			ActiveControlInput = null;
		}
	}
}