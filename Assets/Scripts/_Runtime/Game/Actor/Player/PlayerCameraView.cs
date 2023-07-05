using UnityEngine;

namespace Personal.Character.Player
{
	public class PlayerCameraView : MonoBehaviour
	{
		[SerializeField] Transform fpsInventoryView = null;
		[SerializeField] Transform flashlight = null;

		public Transform FpsInventoryView { get => fpsInventoryView; }
		public Transform Flashlight { get => flashlight; }
	}
}