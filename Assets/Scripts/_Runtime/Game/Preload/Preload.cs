using UnityEngine;
using UnityEngine.SceneManagement;

namespace Personal.Preload
{
	public class Preload : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static void RuntimeInit()
		{
			// Scene index 0 should always be the preload scene.
			SceneManager.LoadScene(0, LoadSceneMode.Additive);
		}
	}
}