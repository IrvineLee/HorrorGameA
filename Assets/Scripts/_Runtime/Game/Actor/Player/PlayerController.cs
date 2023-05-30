using UnityEngine;

using Personal.FSM.Character;

namespace Personal.Character.Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] PlayerStateMachine fsm = null;
		[SerializeField] PlayerInventory inventory = null;

		public PlayerStateMachine FSM { get => fsm; }
		public PlayerInventory Inventory { get => inventory; }
	}
}