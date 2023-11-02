using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class SetPlayerLookAtState : ActorStateBase
	{
		[SerializeField] Transform lookAtTarget = null;

		PlayerStateMachine playerFSM;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerFSM = StageManager.Instance.PlayerController.FSM;
			playerFSM.SetLookAtTarget(lookAtTarget);
			playerFSM.SwitchToState(typeof(PlayerLookAtState)).Forget();

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending, cancellationToken: this.GetCancellationTokenOnDestroy());
		}
	}
}