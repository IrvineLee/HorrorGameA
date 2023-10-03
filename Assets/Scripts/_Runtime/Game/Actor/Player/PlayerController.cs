using UnityEngine;

using Personal.FSM.Character;
using Personal.Manager;
using Personal.Character.Animation;

namespace Personal.Character.Player
{
	public class PlayerController : ActorController
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] FPSController fpsController = null;
		[SerializeField] PlayerInventory inventory = null;
		[SerializeField] PlayerAnimatorController playerAnimatorController = null;

		public PlayerStateMachine FSM { get => fsm; }
		public FPSController FPSController { get => fpsController; }
		public PlayerInventory Inventory { get => inventory; }
		public PlayerAnimatorController PlayerAnimatorController { get => playerAnimatorController; }

		protected override void Initialize()
		{
			StageManager.Instance.RegisterPlayer(this);
		}

		public void PauseFSM(bool isFlag)
		{
			FSM.PauseStateMachine(isFlag);
			FPSController.enabled = !isFlag;
		}
	}
}