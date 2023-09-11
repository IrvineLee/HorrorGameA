using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Transition;

namespace Personal.Preloader
{
	public class Preload : MonoBehaviour
	{
		public static bool IsLoaded { get; private set; }

		static string bootSceneName = SceneName.Boot;
		static string preloadSceneName = SceneName.PreloadScene;
		static string startSceneName = SceneName.Title;

		static string activeSceneName = bootSceneName;

		// Load the boot and preload scene first.
		// Start up the current active scene when in the editor.
		// Otherwise it loads up Title scene.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static async void BeforeRuntimeInit()
		{
#if UNITY_EDITOR
			activeSceneName = SceneManager.GetActiveScene().name;
#endif

			if (!activeSceneName.Equals(bootSceneName))
			{
				SceneManager.LoadScene(bootSceneName);
				await UniTask.NextFrame();
			}
			else
			{
				activeSceneName = startSceneName;
			}

			SceneManager.LoadScene(preloadSceneName, LoadSceneMode.Additive);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static async void RuntimeInit()
		{
			// Makes sure all the singletons are ready to go before loading up the active scene.
			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			Action afterLoadSceneAct = () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeSceneName));
			GameSceneManager.Instance.ChangeLevel(activeSceneName, TransitionType.Fade, TransitionPlayType.Out, default, 0, false, afterLoadSceneAct);

			IsLoaded = true;
		}
	}
}