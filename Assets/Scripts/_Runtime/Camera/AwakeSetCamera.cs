using UnityEngine;

using Personal.Manager;
using Personal.GameState;

namespace Personal.Character
{
	public class AwakeSetCamera : GameInitialize
	{
		protected override void InitializeFirst()
		{
			StageManager.Instance?.SetMainCameraTransform(transform);
		}
	}
}