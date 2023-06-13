using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

using Personal.GameState;
using Personal.Manager;

namespace Personal.UI
{
	public class OnAwakeSetCamera : GameInitialize
	{
		protected override void Initialize()
		{
			if (GameSceneManager.Instance.IsMainScene())
			{
				InitialSetup();
				return;
			}

			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void InitialSetup()
		{
			// Add the camera to main stack.
			var canvas = GetComponentInChildren<Canvas>();
			var cameraData = StageManager.Instance.MainCamera.GetUniversalAdditionalCameraData();

			cameraData.cameraStack.Add(canvas.worldCamera);
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (string.Equals(name, SceneName.Main)) return;

			InitialSetup();
		}

		void OnApplicationQuit()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}
}
