using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class SetPlayerMoveState : StateWithID
	{
		[SerializeField] float turnTowardsSpeed = 1f;

		PlayerController playerController;
		StateBase previousState;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerController = StageManager.Instance.PlayerController;
			previousState = playerController.FSM.CurrentState;

			PlayerMoveToInfo playerMoveToInfo = new PlayerMoveToInfo(moveToTargetTrans, lookAtTargetTrans, turnTowardsSpeed);
			await playerController.MoveTo(playerMoveToInfo);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			playerController.FSM.IFSMHandler?.OnBegin(previousState.GetType());
		}
	}
}