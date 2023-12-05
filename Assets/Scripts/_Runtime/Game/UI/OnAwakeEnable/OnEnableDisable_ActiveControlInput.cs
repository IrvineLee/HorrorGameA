using UnityEngine;

using Personal.InputProcessing;
using Personal.GameState;

namespace Personal.UI
{
	public class OnEnableDisable_ActiveControlInput : GameInitialize
	{
		protected override void OnEnabled()
		{
			ControlInputBase.DisableSaveActiveControl();
		}

		protected override void OnDisabled()
		{
			ControlInputBase.EnableLoadActiveControl();
		}
	}
}
