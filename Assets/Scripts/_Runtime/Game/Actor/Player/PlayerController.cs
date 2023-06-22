using UnityEngine;

using Personal.FSM.Character;
using Personal.GameState;
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

		protected override void PreInitialize()
		{
			StageManager.Instance.RegisterPlayer(this);
		}
	}
}