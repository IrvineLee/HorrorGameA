using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class TeleportState : StateBase
	{
		[Tooltip("Move to target.")]
		[SerializeField] Transform toTarget = null;

		[Tooltip("Look at target.")]
		[SerializeField] Transform lookAtTarget = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			Transform playerTrans = StageManager.Instance.PlayerController.transform;
			playerTrans.position = toTarget.position;

			playerTrans.LookAt(lookAtTarget);

			// The reason you do this is because the player transform only rotates on the y-axis.
			// The other axis is controlled by the child (Done by First Person Controller in-store)
			Vector3 euler = playerTrans.eulerAngles.With(x: 0, z: 0);
			playerTrans.rotation = Quaternion.Euler(euler);

		}
	}
}