
using UnityEngine;

using Helper;
using Personal.GameState;
using Personal.Manager;

namespace Personal.InteractiveObject
{
	public class LookAtPlayer : GameInitialize
	{
		Transform playerTrans;

		protected override void Initialize()
		{
			playerTrans = StageManager.Instance.PlayerController.transform;
		}

		void LateUpdate()
		{
			transform.LookAt(playerTrans);
			transform.localRotation = Quaternion.Euler(transform.eulerAngles.With(x: 0));
		}
	}
}