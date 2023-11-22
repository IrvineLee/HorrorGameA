using UnityEngine;

using Personal.GameState;

namespace Personal.InputProcessing
{
	public abstract class ControlInputBase : GameInitialize, IControlInput
	{
		public static ControlInputBase ActiveControlInput { get; protected set; }

		public IControlInput IControlInput { get => this; }

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