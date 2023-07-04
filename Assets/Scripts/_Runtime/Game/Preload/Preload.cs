using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.Preloader
{
	public class Preload : MonoBehaviour
	{
		public static bool IsLoaded { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static async void RuntimeInit()
		{
			string activeSceneName = SceneManager.GetActiveScene().name;

			SceneManager.LoadScene("Boot");
			await UniTask.NextFrame();

			// Make sure to change the name when scene's name changed.
			SceneManager.LoadScene("PreloadScene", LoadSceneMode.Additive);
			await UniTask.NextFrame();

			GameSceneManager.Instance.ChangeLevel(activeSceneName);
			IsLoaded = true;
		}
	}
}