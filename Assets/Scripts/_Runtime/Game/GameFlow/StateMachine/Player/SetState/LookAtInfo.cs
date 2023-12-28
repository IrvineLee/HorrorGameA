using UnityEngine;

namespace Personal.FSM.Character
{
	public class LookAtInfo
	{
		[SerializeField] Transform lookAt = null;
		[SerializeField] bool isPersist = false;
		[SerializeField] bool isInstant = false;

		public Transform LookAt { get => lookAt; set => lookAt = value; }
		public bool IsPersist { get => isPersist; }                         // Does the player remain looking at lookAt after state end?
		public bool IsInstant { get => isInstant; }                         // Does it turn instantly or by animation?

		public LookAtInfo() { }

		public LookAtInfo(Transform lookAt, bool isPersist = false, bool isInstant = true)
		{
			this.lookAt = lookAt;
			this.isPersist = isPersist;
			this.isInstant = isInstant;
		}
	}
}