using UnityEngine;

using Personal.GameState;

namespace Personal.Character
{
	public class ActorController : GameInitialize
	{
		[SerializeField] Transform head = null;

		public Transform Head { get => head; }
	}
}