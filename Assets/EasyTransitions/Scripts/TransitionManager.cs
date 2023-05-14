using System;
using System.Collections.Generic;
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

		Dictionary<TransitionType, Transition> transitionDictionary = new();

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
		/// <param name="delay"></param>
		/// <param name="inBetweenAction"></param>
		public void Transition(TransitionType transitionType, float delay, Action inBetweenAction = default)
		{
			if (isRunningTransition) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, delay, transitionSettings).Forget();
		}

		async UniTask HandleTransition(TransitionType transitionType, float delay, TransitionSettings transitionSettings)
		{
			isRunningTransition = true;
			await UniTask.Delay((int)delay.SecondsToMilliseconds());

			int transitionDuration = 0;
			if (!transitionDictionary.TryGetValue(transitionType, out Transition transition))
				transitionDuration = await SpawnTemplate(transitionType, transitionSettings);
			else
				transitionDuration = TransitionSetup(transition, transitionSettings);

			await UniTask.Delay(transitionDuration);
			isRunningTransition = false;
		}

		async UniTask<int> SpawnTemplate(TransitionType transitionType, TransitionSettings transitionSettings)
		{
			GameObject template = await AddressableHelper.Spawn(AssetAddress.TransitionTemplate);

			Transition transition = template.GetComponent<Transition>();
			transitionDictionary.Add(transitionType, transition);

			return TransitionSetup(transition, transitionSettings);
		}

		int TransitionSetup(Transition transition, TransitionSettings transitionSettings)
		{
			transition.Begin(transitionSettings, transitionManagerSettings);

			// Canvas setup.
			canvasScaler.referenceResolution = transitionSettings.ReferenceResolution;
			canvasGroup.blocksRaycasts = transitionSettings.BlockRaycasts;

			float duration = transitionSettings.TransitionTime;
			if (transitionSettings.AutoAdjustTransitionTime)
				duration = duration / transitionSettings.TransitionSpeed;

			return (int)duration.SecondsToMilliseconds();
		}
	}
}
