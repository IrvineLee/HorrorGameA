using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

using Cinemachine;
using Personal.Character.Player;

namespace Personal.Character
{
	public class CameraHandler : MonoBehaviour
	{
		public Camera MainCamera { get; private set; }
		public PlayerCameraView PlayerCameraView { get; private set; }
		public PhysicsRaycaster PhysicsRaycaster { get; private set; }
		public CinemachineBrain CinemachineBrain { get; private set; }
		public UniversalAdditionalCameraData UniversalAdditionalCameraData { get; private set; }

		void Awake()
		{
			MainCamera = Camera.main;
			PlayerCameraView = GetComponentInChildren<PlayerCameraView>();
			PhysicsRaycaster = GetComponentInChildren<PhysicsRaycaster>();
			CinemachineBrain = GetComponentInChildren<CinemachineBrain>();
			UniversalAdditionalCameraData = GetComponentInChildren<UniversalAdditionalCameraData>();
		}

		public void SetPosAndRot(Transform target)
		{
			SetPosAndRot(target.position, target.rotation);
		}

		public void ResetPosAndRot()
		{
			SetPosAndRot(Vector3.zero, Quaternion.identity);
		}

		void SetPosAndRot(Vector3 position, Quaternion rotation)
		{
			// Set camera transform to MainCamera.
			MainCamera.transform.position = position;
			MainCamera.transform.rotation = rotation;
		}
	}
}