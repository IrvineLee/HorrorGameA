using UnityEngine;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.Transition;
using Personal.CanvasUI;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.FSM.Character
{
	public class FadeState : StateBase
	{
		[SerializeField] TransitionType transitionType = TransitionType.Fade;
		[SerializeField] TransitionPlayType transitionPlayType = TransitionPlayType.All;
		[SerializeField] CanvasSortOrder transitionBelowSortOrder = CanvasSortOrder.Transition;

		ActionMapType defaultActionMapType;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			TransitionManager.Instance.SetCanvasSortOrder(transitionBelowSortOrder);
			TransitionManager.Instance.Transition(transitionType, transitionPlayType);

			// You don't want the player to pause the game when it's fading.
			defaultActionMapType = InputManager.Instance.CurrentActionMapType;
			InputManager.Instance.EnableActionMap(ActionMapType.UI);
		}

		public override async UniTask OnExit()
		{
			await base.OnExit();
			InputManager.Instance.EnableActionMap(defaultActionMapType);
		}

		public override async UniTask Standby()
		{
			await UniTask.WaitUntil(() => DialogueManager.Instance && !DialogueManager.Instance.isConversationActive);

			TransitionManager.Instance.ResetCanvasSortOrder();
		}
	}
}