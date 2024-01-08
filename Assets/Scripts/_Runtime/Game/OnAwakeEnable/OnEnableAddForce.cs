using UnityEngine;

using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class OnEnableAddForce : GameInitialize
	{
		[SerializeField] Vector3 direction = Vector3.zero;
		[SerializeField] float force = 1f;
		[SerializeField] ForceMode forceMode = ForceMode.Force;

		Rigidbody rgBody;

		protected override void EarlyInitialize()
		{
			rgBody = GetComponentInChildren<Rigidbody>();
		}

		void OnEnable()
		{
			if (!rgBody) return;

			rgBody.AddForce(direction * force, forceMode);
		}
	}
}