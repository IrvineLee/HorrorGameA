using UnityEngine;

using Personal.FSM.Character;
using Personal.Manager;

namespace Personal.Character.Player
{
	public class PlayerController : ActorController
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] FPSController fpsController = null;
		[SerializeField] PlayerInventory inventory = null;

		public PlayerStateMachine FSM { get => fsm; }
		public FPSController FPSController { get => fpsController; }
		public PlayerInventory Inventory { get => inventory; }
		public PlayerCameraView PlayerCameraView { get; private set; }

		protected override void PreInitialize()
		{
			StageManager.Instance.RegisterPlayer(this);
		}

		public void RegisterCameraView(PlayerCameraView playerCameraView)
		{
			PlayerCameraView = playerCameraView;
		}
	}
}