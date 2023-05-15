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

			Transition(TransitionType.Fade, TransitionPlayType.Out);
		}

		/// <summary>
		/// Loads the new Scene asyncronosly with a transition.
		/// </summary>
		/// <param name="transitionType"></param>
		/// <param name="delay"></param>
		/// <param name="inBetweenAction"></param>
		public void Transition(TransitionType transitionType, TransitionPlayType transitionPlayType = TransitionPlayType.All, float delay = 0, Action inBetweenAction = default)
		{
			if (isRunningTransition) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, transitionPlayType, delay, transitionSettings, inBetweenAction).Forget();
		}

		async UniTask HandleTransition(TransitionType transitionType, TransitionPlayType transitionPlayType, float delay, TransitionSettings transitionSettings, Action inBetweenAction)
		{
			isRunningTransition = true;
			await UniTask.Delay((int)delay.SecondsToMilliseconds());

			if (!transitionDictionary.TryGetValue(transitionType, out Transition transition))
				transition = await SpawnTemplate(transitionType, transitionSettings, inBetweenAction);

			await TransitionSetup(transition, transitionPlayType, transitionSettings, inBetweenAction);

			isRunningTransition = false;
		}

		async UniTask<Transition> SpawnTemplate(TransitionType transitionType, TransitionSettings transitionSettings, Action inBetweenAction)
		{
			GameObject template = await AddressableHelper.Spawn(transitionType.GetStringValue(), Vector3.zero, CanvasGroup.transform);

			Transition transition = template.GetComponent<Transition>();
			transitionDictionary.Add(transitionType, transition);

			return transition;
		}

		async UniTask TransitionSetup(Transition transition, TransitionPlayType transitionPlayType, TransitionSettings transitionSettings, Action inBetweenAction)
		{
			// Canvas setup.
			canvasScaler.referenceResolution = transitionSettings.ReferenceResolution;
			canvasGroup.blocksRaycasts = transitionSettings.BlockRaycasts;

			await transition.Begin(transitionSettings, transitionPlayType, transitionManagerSettings, inBetweenAction);
		}
	}
}
