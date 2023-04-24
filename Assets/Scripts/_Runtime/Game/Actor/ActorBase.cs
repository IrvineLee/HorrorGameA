using UnityEngine;

namespace Personal.Character
{
	public class ActorBase : MonoBehaviour
	{
		protected Transform Actor { get; private set; }

		public void SetActor(Transform actor)
		{
			Actor = actor;
		}

		protected virtual void HandleMovement() { }
	}
}