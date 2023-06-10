using UnityEngine;

using Personal.FSM.Character;
using Personal.GameState;

namespace Personal.Character.Player
{
	public class PlayerController : GameInitialize
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] PlayerInventory inventory = null;

		public PlayerStateMachine FSM { get => fsm; }
		public PlayerInventory Inventory { get => inventory; }
	}
}