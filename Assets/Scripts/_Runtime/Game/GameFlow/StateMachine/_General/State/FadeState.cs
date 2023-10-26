using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.Transition;

namespace Personal.FSM.Character
{
	public class FadeState : StateBase
	{
		[SerializeField] TransitionType transitionType = TransitionType.Fade;
		[SerializeField] TransitionPlayType transitionPlayType = TransitionPlayType.All;
		[SerializeField] int transitionSortOrder = 999;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			TransitionManager.Instance.SetCanvasSortOrder(transitionSortOrder);
			TransitionManager.Instance.Transition(transitionType, transitionPlayType);
		}

		public override async UniTask Standby()
		{
			await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);

			TransitionManager.Instance.ResetCanvasSortOrder();
			Debug.Log("Ended");
		}
	}
}