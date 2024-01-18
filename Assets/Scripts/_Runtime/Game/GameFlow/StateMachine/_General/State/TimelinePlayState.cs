using UnityEngine;
using UnityEngine.Playables;

using Helper;
using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class TimelinePlayState : StateBase
	{
		[SerializeField] PlayableDirector playableDirector = null;

		PlayerController playerController;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerController = StageManager.Instance.PlayerController;
			playerController.MoveStart(true);

			playerController.Animator.enabled = true;

			playableDirector.gameObject.SetActive(true);
			float duration = (float)(playableDirector.duration + playableDirector.initialTime);

			await UniTask.NextFrame();
			await UniTask.Delay(duration.SecondsToMilliseconds(), cancellationToken: this.GetCancellationTokenOnDestroy());

			FPSController fpsController = playerController.FPSController;
			float pitch = fpsController.CinemachineCameraGO.transform.rotation.eulerAngles.x;
			fpsController.UpdateTargetPitch(pitch);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();

			playerController.Animator.enabled = false;
			playerController.MoveStart(false);
		}
	}
}