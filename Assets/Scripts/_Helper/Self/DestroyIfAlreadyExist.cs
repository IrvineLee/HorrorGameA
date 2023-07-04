using System.Linq;
using UnityEngine;

namespace Helper
{
	public class DestroyIfAlreadyExist : MonoBehaviour
	{
		[SerializeField] MonoBehaviour script = null;

		void Awake()
		{
			var scriptList = FindObjectsOfType(script.GetType()).ToList();

			if (scriptList.Count > 1)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
		}
	}
}
