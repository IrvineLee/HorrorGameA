using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Character
{
	public class AwakeSetCamera : GameInitialize
	{
		protected override void EarlyInitialize()
		{
			StageManager.Instance?.SetMainCameraTransform(transform);
		}
	}
}