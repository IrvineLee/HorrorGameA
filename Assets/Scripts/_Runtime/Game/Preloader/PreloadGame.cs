using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Transition;

namespace Personal.Preloader
{
	public class PreloadGame : Preload
	{
		const string BootSceneName = SceneName.Boot;
		const string preloadSceneName = SceneName.PreloadScene;
		const string startSceneName = SceneName.Title;

		static string activeSceneName = BootSceneName;

		// Load the boot and preload scene first.
		// Start up the current active scene when in the editor.
		// Otherwise it loads up Title scene.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static async void BeforeRuntimeInit()
		{
#if UNITY_EDITOR
			activeSceneName = SceneManager.GetActiveScene().name;
#endif

			if (!activeSceneName.Equals(BootSceneName))
			{
				SceneManager.LoadScene(BootSceneName);
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
			IsPreloadSceneLoaded = true;

			// Makes sure all the singletons are ready to go before loading up the active scene.
			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			Action afterLoadSceneAct = () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeSceneName));
			GameSceneManager.Instance.ChangeLevel(activeSceneName, TransitionType.Fade, TransitionPlayType.Out, default, 0, false, afterLoadSceneAct);

			IsLoaded = true;
		}
	}
}