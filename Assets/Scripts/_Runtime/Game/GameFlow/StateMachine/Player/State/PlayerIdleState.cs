using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerIdleState : PlayerBaseState
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			// Somewhere else will change the state of player.
			PlayerStateMachine playerFSM = StageManager.Instance.PlayerController.FSM;
			await UniTask.WaitUntil(() => !playerFSM.IsPlayerThisState(typeof(PlayerIdleState)), cancellationToken: this.GetCancellationTokenOnDestroy());
		}
	}
}