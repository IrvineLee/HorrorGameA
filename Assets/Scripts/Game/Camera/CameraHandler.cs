using UnityEngine;

using Cinemachine;

namespace Personal.CameraView
{
	public class CameraHandler : MonoBehaviour
	{
		[SerializeField] float fov;

		CinemachineVirtualCamera cam;
		float newFOV;

		void Awake()
		{
			Initialize();
		}

		public void Initialize()
		{
			Camera mainCam = Camera.main;
			newFOV = fov / ((float)mainCam.pixelWidth / mainCam.pixelHeight);

			// Update camera FOV.
			cam = GetComponentInChildren<CinemachineVirtualCamera>();
			cam.m_Lens.FieldOfView = newFOV;
		}
	}
}