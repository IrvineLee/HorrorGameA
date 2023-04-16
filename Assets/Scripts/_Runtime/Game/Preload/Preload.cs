using UnityEngine;
using UnityEngine.SceneManagement;

namespace Personal.Preload
{
	public class Preload : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static void RuntimeInit()
		{
			// Make sure to change the name when scene's name changed.
			SceneManager.LoadScene("PreloadScene", LoadSceneMode.Additive);
		}
	}
}