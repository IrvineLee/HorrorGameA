using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Manager;
using Personal.GameState;
using Personal.Transition;
using Personal.CanvasUI;
using Personal.Constant;
using Personal.Definition;

namespace Personal.InteractiveObject
{
	public class EndGameHandler : GameInitialize
	{
		[SerializeField] TransitionType transitionType = TransitionType.Fade;
		[SerializeField] TransitionPlayType transitionPlayType = TransitionPlayType.All;
		[SerializeField] CanvasSortOrder transitionBelowSortOrder = CanvasSortOrder.Transition;

		[Space]
		[SerializeField] Transform endingTrans = null;
		[SerializeField] float delayBeforeEndingTrans = 2f;

		[Tooltip("Delay duration after endingTrans is enabled.")]
		[SerializeField] float endingDuration = 5f;

		[Space]
		[SerializeField] SceneType sceneType = SceneType.Title;
		[SerializeField] float delayBeforeTransition = 0;

		async void OnEnable()
		{
			StageManager.Instance.PlayerController.PauseFSM(true);
			InputManager.Instance.DisableAllActionMap();

			CursorManager.Instance.SetCenterCrosshair(CursorDefinition.CrosshairType.Nothing);
			TransitionManager.Instance.SetCanvasSortOrder(transitionBelowSortOrder);

			await ShowEnding();
			await UniTask.Delay(delayBeforeTransition.SecondsToMilliseconds(), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

			TransitionManager.Instance.ResetCanvasSortOrder();
			GameSceneManager.Instance.ChangeLevel(sceneType.GetStringValue(), transitionType, transitionPlayType, isIgnoreTimescale: false);
		}

		async UniTask ShowEnding()
		{
			await UniTask.Delay(delayBeforeEndingTrans.SecondsToMilliseconds(), cancellationToken: gameObject.GetCancellationTokenOnDestroy());
			endingTrans?.gameObject.SetActive(true);
			await UniTask.Delay(endingDuration.SecondsToMilliseconds(), cancellationToken: gameObject.GetCancellationTokenOnDestroy());
		}
	}
}

