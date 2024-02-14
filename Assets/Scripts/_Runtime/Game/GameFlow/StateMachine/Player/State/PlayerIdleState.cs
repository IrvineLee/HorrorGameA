using Cysharp.Threading.Tasks;

using Personal.Character.Player;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerIdleState : PlayerBaseState
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			RunActorAnimation();

			// Somewhere else will change the state of player.
			PlayerController playerController = StageManager.Instance.PlayerController;
			playerController.PauseFSM(true);

			await UniTask.WaitUntil(() => !playerController.FSM.IsPlayerThisState(typeof(PlayerIdleState)), cancellationToken: this.GetCancellationTokenOnDestroy());
			playerController.PauseFSM(false);
		}
	}
}