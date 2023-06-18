using UnityEngine;

namespace HyperCasualGame.Bullet
{
	public class OnAwakeDisableMouse : MonoBehaviour
	{
		void Awake()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}
	}
}