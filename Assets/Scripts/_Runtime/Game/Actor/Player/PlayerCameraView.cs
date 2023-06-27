using UnityEngine;

using Personal.GameState;
using Personal.Manager;

namespace Personal.Character.Player
{
	public class PlayerCameraView : GameInitialize
	{
		[SerializeField] Transform fpsInventoryView = null;
		[SerializeField] Transform flashlight = null;

		public Transform FpsInventoryView { get => fpsInventoryView; }
		public Transform Flashlight { get => flashlight; }

		protected override void Initialize()
		{
			StageManager.Instance.PlayerController.RegisterCameraView(this);
		}
	}
}