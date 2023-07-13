using UnityEngine;

using Personal.Character.Player;
using Personal.Manager;

namespace Personal.Character
{
	public class CameraHandler : MonoBehaviour
	{
		public PlayerCameraView PlayerCameraView { get; private set; }

		void Start()
		{
			var cam = GetComponentInChildren<Camera>();
			if (!cam) return;

			PlayerCameraView = GetComponentInChildren<PlayerCameraView>();
			StageManager.Instance.RegisterCamera(cam);
		}
	}
}