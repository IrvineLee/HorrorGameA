using UnityEngine;

using Personal.Preloader;

namespace Helper
{
	public class App : MonoBehaviour
	{
		public static bool IsQuitting { get; private set; }
		public static bool IsReadyToStart { get => Preload.IsPreloadSceneLoaded; }

		void OnApplicationQuit()
		{
			IsQuitting = true;
		}
	}
}