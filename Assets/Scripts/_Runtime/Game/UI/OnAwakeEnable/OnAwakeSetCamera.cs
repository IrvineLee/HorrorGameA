using UnityEngine;
using UnityEngine.Rendering.Universal;

using Personal.GameState;
using Personal.Manager;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class OnAwakeSetCamera : GameInitialize
	{
		protected override async void OnMainScene()
		{
			await UniTask.NextFrame();

			InitialSetup();
		}

		void InitialSetup()
		{
			// Add the camera to main stack.
			var canvas = GetComponentInChildren<Canvas>();
			var cameraData = StageManager.Instance.MainCamera.GetUniversalAdditionalCameraData();

			cameraData.cameraStack.Add(canvas.worldCamera);
		}
	}
}
