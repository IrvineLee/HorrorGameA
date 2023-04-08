using UnityEngine;

namespace Personal.Preload
{
	public class Preload : MonoBehaviour
	{
		const string PATH = "GameManager";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static void RuntimeInit()
		{
			var pathGO = Resources.Load<GameObject>(PATH);
			GameObject go = Instantiate(pathGO);
			go.name = pathGO.name;
		}
	}
}