using UnityEngine;

using Helper;
using Personal.Pool;

namespace Personal.Manager
{
	public class DebugManager : MonoBehaviourSingleton<DebugManager>
	{
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				SceneManager.Instance.ChangeLevel(1);
				//FindObjectOfType<SpawnablePoolBase>().Get();
			}
		}
	}
}

