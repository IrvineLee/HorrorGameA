using UnityEngine;

namespace Helper
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
