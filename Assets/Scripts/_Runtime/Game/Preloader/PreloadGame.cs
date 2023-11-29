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
		const string BOOT_SCENE_NAME = SceneName.Boot;
		const string PRELOAD_SCENE_NAME = SceneName.PreloadScene;
		const string START_SCENE_NAME = SceneName.Title;

		public static string PreviousSceneName { get; private set; }

		static string activeSceneName = BOOT_SCENE_NAME;

		// Load the boot and preload scene first.
		// Start up the current active scene when in the editor.
		// Otherwise it loads up Title scene.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static async void BeforeRuntimeInit()
		{
#if UNITY_EDITOR
			activeSceneName = SceneManager.GetActiveScene().name;
#endif

			if (!activeSceneName.Equals(BOOT_SCENE_NAME))
			{
				SceneManager.LoadScene(BOOT_SCENE_NAME);
				await UniTask.NextFrame();
			}
			else
			{
				activeSceneName = START_SCENE_NAME;
			}

			SceneManager.LoadScene(PRELOAD_SCENE_NAME, LoadSceneMode.Additive);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static async void RuntimeInit()
		{
			IsPreloadSceneLoaded = true;
			PreviousSceneName = activeSceneName;

			// Makes sure all the singletons are ready to go before loading up the active scene.
			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			Action afterLoadSceneAct = () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeSceneName));
			GameSceneManager.Instance.ChangeLevel(activeSceneName, TransitionType.Fade, TransitionPlayType.Out, default, 0, false, afterLoadSceneAct);

			IsLoaded = true;
		}
	}
}