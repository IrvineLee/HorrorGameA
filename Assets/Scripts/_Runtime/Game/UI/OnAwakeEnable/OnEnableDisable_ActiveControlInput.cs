using UnityEngine;

using Personal.InputProcessing;
using Personal.GameState;

namespace Personal.UI
{
	public class OnEnableDisable_ActiveControlInput : GameInitialize
	{
		protected override void OnEnabled()
		{
			ControlInputBase.StopControl(true);
		}

		protected override void OnDisabled()
		{
			ControlInputBase.StopControl(false);
		}
	}
}
