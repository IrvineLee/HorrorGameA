using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.FSM;
using Personal.FSM.Character;
using Personal.Manager;
using Personal.GameState;
using Personal.Transition;
using Personal.CanvasUI;

namespace Personal.InteractiveObject
{
	public class MainSceneHandler : GameInitialize
	{
		[Tooltip("The state machine when the screen is all black.")]
		[SerializeField] OrderedStateMachine blackScreenStateMachine = null;

		[Tooltip("The state machine after fading into the scene.")]
		[SerializeField] OrderedStateMachine initStateMachine = null;

		[Tooltip("The delay before starting initStateMachine.")]
		[SerializeField] float delayBeforeInit = 0;

		PlayerStateMachine playerFSM;

		protected async override void OnMainScene()
		{
			// Since there is a fade out sequence when you start, put the black screen to begin with it.
			UIManager.Instance.ToolsUI.BlackScreen.gameObject.SetActive(true);

			// You want to wait other OnMainScene to get called first before this.
			await UniTask.NextFrame();

			playerFSM = StageManager.Instance.PlayerController.FSM;

			// Stop the player's movement.
			InputManager.Instance.EnableActionMap(InputProcessing.ActionMapType.UI);

			await HandleBlackScreen();
			await HandleBlackScreenStateMachine();

			TransitionManager.Instance.Transition(TransitionType.Fade, TransitionPlayType.Out);

			await UniTask.WaitUntil(() => !StageManager.Instance.IsBusy);
			TransitionManager.Instance.ResetCanvasSortOrder();

			await UniTask.Delay(delayBeforeInit.SecondsToMilliseconds());

			await HandleInitStateMachine();
			InputManager.Instance.EnableActionMap(InputProcessing.ActionMapType.Player);
		}

		async UniTask HandleBlackScreen()
		{
			// Make the transition to black, so you can fade out when you want to.
			TransitionManager.Instance.ResetTransition();
			TransitionManager.Instance.SetCanvasSortOrder(CanvasSortOrder.Dialogue);
			TransitionManager.Instance.Transition(TransitionType.Fade, TransitionPlayType.In);

			// Make sure the transition is fully black before disabling the black screen.
			await UniTask.WaitUntil(() => !StageManager.Instance.IsBusy);
			UIManager.Instance.ToolsUI.BlackScreen.enabled = false;
		}

		async UniTask HandleBlackScreenStateMachine()
		{
			if (!blackScreenStateMachine) return;

			var interactionAssign = blackScreenStateMachine.GetComponentInChildren<InteractionAssign>();
			await blackScreenStateMachine.Begin(playerFSM, null, interactionAssign);
		}

		async UniTask HandleInitStateMachine()
		{
			if (!initStateMachine) return;

			var interactionAssign = initStateMachine.GetComponentInChildren<InteractionAssign>();
			await initStateMachine.Begin(playerFSM, null, interactionAssign);
		}
	}
}

