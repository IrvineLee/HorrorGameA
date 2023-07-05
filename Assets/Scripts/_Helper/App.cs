using UnityEngine;

namespace Helper
{
	public class App : MonoBehaviour
	{
		public static bool IsQuitting { get; private set; }

		void OnApplicationQuit()
		{
			IsQuitting = true;
		}
	}
}