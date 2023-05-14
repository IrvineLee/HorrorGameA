using System;
using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Helper;
using Personal;

namespace EasyTransition
{
	public class TransitionManager : GameInitializeSingleton<TransitionManager>
	{
		[Tooltip("The Transition Manager Settings store all the premade and your transitions.")]
		[SerializeField] TransitionManagerSettings transitionManagerSettings;

		[SerializeField] GameObject transitionTemplate;
		[SerializeField] CanvasScaler canvasScaler;

		public TransitionManagerSettings TransitionManagerSettings { get => transitionManagerSettings; }
		public CanvasGroup CanvasGroup { get => canvasGroup; }

		CanvasGroup canvasGroup;
		bool isRunningTransition;

		protected override async UniTask Awake()
		{
			await base.Awake();

			canvasGroup = GetComponentInChildren<CanvasGroup>();
			transitionManagerSettings.Initialize();
		}

		/// <summary>
		/// Loads the new Scene asyncronosly with a transition.
		/// </summary>
		/// <param name="transitionType"></param>
		/// <param name="loadDelay"></param>
		/// <param name="inBetweenAction"></param>
		public void Transition(TransitionType transitionType, float loadDelay, Action inBetweenAction = default)
		{
			if (isRunningTransition) return;

			HandleTransition(loadDelay, TransitionManagerSettings.GetTransitionSetting(transitionType)).Forget();
		}

		async UniTask HandleTransition(float delay, TransitionSettings transitionSettings)
		{
			isRunningTransition = true;
			await UniTask.Delay((int)delay.SecondsToMilliseconds());

			int transitionDuration = await SpawnTemplate(transitionSettings);

			await UniTask.Delay(transitionDuration);
			isRunningTransition = false;
		}

		async UniTask<int> SpawnTemplate(TransitionSettings transitionSettings)
		{
			GameObject template = await AddressableHelper.Spawn(AssetAddress.TransitionTemplate);

			Transition transition = template.GetComponent<Transition>();
			transition.Initialize(transitionSettings, transitionManagerSettings);

			// Canvas initialize.
			canvasScaler.referenceResolution = transitionSettings.ReferenceResolution;
			canvasGroup.blocksRaycasts = transitionSettings.BlockRaycasts;

			float duration = transitionSettings.TransitionTime;
			if (transitionSettings.AutoAdjustTransitionTime)
				duration = duration / transitionSettings.TransitionSpeed;

			return (int)duration.SecondsToMilliseconds();
		}
	}
}
