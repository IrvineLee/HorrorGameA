using UnityEngine;

namespace Helper
{
	public class DestroyOnLoad : MonoBehaviour
	{
		void Awake()
		{
			Destroy(gameObject);
		}
	}
}
