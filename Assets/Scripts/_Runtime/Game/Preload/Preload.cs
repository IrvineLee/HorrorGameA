using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Constant;
using Helper;

namespace Personal.Preloader
{
	public class Preload : MonoBehaviour
	{
		public static bool IsLoaded { get; private set; }

		static string bootSceneName = SceneType.Boot.GetStringValue();
		static string preloadSceneName = SceneType.PreloadScene.GetStringValue();
		static string startSceneName = SceneType.Title.GetStringValue();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static async void RuntimeInit()
		{
			string activeSceneName = SceneManager.GetActiveScene().name;

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
			await UniTask.NextFrame();

			if (!GameManager.IsLoadingOver)
				await UniTask.WaitUntil(() => GameManager.IsLoadingOver);

			GameSceneManager.Instance.ChangeLevel(activeSceneName, transitionPlayType: Transition.TransitionPlayType.Out, isIgnoreTimescale: false);
			IsLoaded = true;
		}
	}
}