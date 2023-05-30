using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerIdleState : ActorStateBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			// Somewhere else will change the state of player.
			PlayerStateMachine playerFSM = StageManager.Instance.PlayerController.FSM;
			await UniTask.WaitUntil(() => playerFSM.CurrentState.GetType() != typeof(PlayerIdleState));
		}
	}
}