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
			PlayerCameraView = GetComponentInChildren<PlayerCameraView>();
			StageManager.Instance.SetMainCameraTransform(transform);
		}
	}
}