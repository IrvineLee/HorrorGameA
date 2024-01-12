using UnityEngine;
using UnityEngine.Playables;

using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public class TimelinePlayState : StateBase
	{
		[SerializeField] PlayableDirector playableDirector = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playableDirector.gameObject.SetActive(true);

			await UniTask.NextFrame();
			await UniTask.WaitUntil(() => playableDirector.state != PlayState.Playing, cancellationToken: this.GetCancellationTokenOnDestroy());
		}
	}
}