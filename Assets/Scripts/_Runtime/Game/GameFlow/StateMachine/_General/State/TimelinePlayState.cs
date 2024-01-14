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

			playableDirector.gameObject.SetActive(true);
			Debug.Break();
			float duration = (float)(playableDirector.duration + playableDirector.initialTime);

			await UniTask.NextFrame();
			await UniTask.Delay(duration.SecondsToMilliseconds(), cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			playerController.MoveStart(false);
		}
	}
}