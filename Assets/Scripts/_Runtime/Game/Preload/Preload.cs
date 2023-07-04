using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.Preloader
{
	public class Preload : MonoBehaviour
	{
		public static bool IsLoaded { get; private set; }

		// Make sure to change the name when scene's name changed.
		static string bootSceneName = "Boot";
		static string preloadSceneName = "PreloadScene";
		static string startSceneName = "Title";

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

			GameSceneManager.Instance.ChangeLevel(activeSceneName, Transition.TransitionType.Fade, Transition.TransitionPlayType.Out);
			IsLoaded = true;
		}
	}
}