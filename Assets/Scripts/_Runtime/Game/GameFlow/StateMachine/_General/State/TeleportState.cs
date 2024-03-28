using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class TeleportState : StateWithID
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			Transform playerTrans = StageManager.Instance.PlayerController.transform;
			playerTrans.position = moveToTargetTrans.position;

			playerTrans.LookAt(lookAtTargetTrans);

			// The reason you do this is because the player transform only rotates on the y-axis.
			// The other axis is controlled by the child (Done by First Person Controller in-store)
			Vector3 euler = playerTrans.eulerAngles.With(x: 0, z: 0);
			playerTrans.rotation = Quaternion.Euler(euler);

		}
	}
}