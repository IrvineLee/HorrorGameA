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
		public void Transition(TransitionType transitionType, float delay = 0, Action inBetweenAction = default)
		{
			if (isRunningTransition) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, delay, transitionSettings).Forget();
		}

		async UniTask HandleTransition(TransitionType transitionType, float delay, TransitionSettings transitionSettings)
		{
			isRunningTransition = true;
			await UniTask.Delay((int)delay.SecondsToMilliseconds());

			if (!transitionDictionary.TryGetValue(transitionType, out Transition transition))
				await SpawnTemplate(transitionType, transitionSettings);
			else
				await TransitionSetup(transition, transitionSettings);

			isRunningTransition = false;
		}

		async UniTask SpawnTemplate(TransitionType transitionType, TransitionSettings transitionSettings)
		{
			GameObject template = await AddressableHelper.Spawn(transitionType.GetStringValue(), Vector3.zero, CanvasGroup.transform);

			Transition transition = template.GetComponent<Transition>();
			transitionDictionary.Add(transitionType, transition);

			await TransitionSetup(transition, transitionSettings);
		}

		async UniTask TransitionSetup(Transition transition, TransitionSettings transitionSettings)
		{
			// Canvas setup.
			canvasScaler.referenceResolution = transitionSettings.ReferenceResolution;
			canvasGroup.blocksRaycasts = transitionSettings.BlockRaycasts;

			await transition.Begin(transitionSettings, transitionManagerSettings);
		}
	}
}
