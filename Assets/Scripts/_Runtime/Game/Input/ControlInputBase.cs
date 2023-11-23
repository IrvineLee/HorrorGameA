using UnityEngine;

using Personal.GameState;

namespace Personal.InputProcessing
{
	public abstract class ControlInputBase : GameInitialize
	{
		public static ControlInputBase ActiveControlInput { get; protected set; }

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