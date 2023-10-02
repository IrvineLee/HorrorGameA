using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Character
{
	public class AwakeSetCamera : GameInitialize
	{
		protected override void Initialize()
		{
			StageManager.Instance?.SetMainCameraTransform(transform);
		}
	}
}