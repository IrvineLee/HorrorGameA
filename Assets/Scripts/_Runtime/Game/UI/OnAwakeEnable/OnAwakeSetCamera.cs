using UnityEngine;
using UnityEngine.Rendering.Universal;

using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class OnAwakeSetCamera : GameInitialize
	{
		protected override void OnPostMainScene()
		{
			// Add the camera to main stack.
			var canvas = GetComponentInChildren<Canvas>();
			var cameraData = StageManager.Instance.MainCamera.GetUniversalAdditionalCameraData();

			cameraData.cameraStack.Add(canvas.worldCamera);
		}
	}
}
