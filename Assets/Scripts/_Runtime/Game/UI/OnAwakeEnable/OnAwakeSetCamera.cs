using UnityEngine;
using UnityEngine.Rendering.Universal;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class OnAwakeSetCamera : GameInitialize
	{
		protected override async UniTask Awake()
		{
			await base.Awake();

			// Add the camera to main stack.
			var canvas = GetComponentInChildren<Canvas>();
			var cameraData = StageManager.Instance.MainCamera.GetUniversalAdditionalCameraData();

			cameraData.cameraStack.Add(canvas.worldCamera);
		}
	}
}
