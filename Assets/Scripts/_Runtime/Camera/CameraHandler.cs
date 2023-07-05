using Personal.Manager;
using UnityEngine;

namespace Personal.Character
{
	public class CameraHandler : MonoBehaviour
	{
		void Awake()
		{
			var cam = GetComponentInChildren<Camera>();
			StageManager.Instance.RegisterCamera(cam);
		}
	}
}