using UnityEngine;

namespace Personal.Character
{
	public class ActorController : MonoBehaviour
	{
		[SerializeField] Transform head = null;

		public Transform Head { get => head; }
	}
}