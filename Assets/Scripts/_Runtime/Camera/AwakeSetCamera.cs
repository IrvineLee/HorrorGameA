using UnityEngine;

using Personal.Manager;

namespace Personal.Character
{
	public class AwakeSetCamera : MonoBehaviour
	{
		void Awake()
		{
			StageManager.Instance?.SetMainCameraTransform(transform);
		}
	}
}