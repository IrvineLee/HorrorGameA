using UnityEngine;

namespace Personal.Preload
{
	public class Preload : MonoBehaviour
	{
		const string PATH_GM = "GameManager";
		const string PATH_POOL = "PoolManager";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static void RuntimeInit()
		{
			Spawn(PATH_POOL);

			// Since GameManager will ensure all the others are already spawned, put it the last.
			Spawn(PATH_GM);
		}

		static void Spawn(string s)
		{
			var pathGO = Resources.Load<GameObject>(s);
			GameObject go = Instantiate(pathGO);
			go.name = pathGO.name + "_Preload";
		}
	}
}