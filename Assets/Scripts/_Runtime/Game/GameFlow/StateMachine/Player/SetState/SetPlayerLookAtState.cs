using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class SetPlayerLookAtState : StateBase
	{
		[SerializeField] Transform lookAtTarget = null;

		[Tooltip("Does the player remain looking at target after state end?")]
		[SerializeField] bool isPersist = false;

		[Tooltip("Does it turn by animation or instantly?")]
		[SerializeField] bool isInstant = true;

		PlayerStateMachine playerFSM;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerFSM = StageManager.Instance.PlayerController.FSM;
			var lookAtInfo = new LookAtInfo(lookAtTarget, isPersist, isInstant);

			playerFSM.SetLookAtInfo(lookAtInfo);
			playerFSM.IFSMHandler?.OnBegin(typeof(PlayerLookAtState));

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => !StageManager.Instance.CameraHandler.CinemachineBrain.IsBlending, cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			playerFSM.SetLookAtInfo(null);
		}
	}
}