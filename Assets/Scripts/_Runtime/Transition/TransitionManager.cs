using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using Helper;

namespace Personal.Transition
{
	public class TransitionManager : MonoBehaviourSingleton<TransitionManager>
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

		protected override UniTask Boot()
		{
			canvasGroup = GetComponentInChildren<CanvasGroup>();
			transitionManagerSettings.Initialize();

			InitialSetup();

			return UniTask.CompletedTask;
		}

		/// <summary>
		/// Loads the new Scene asyncronosly with a transition.
		/// </summary>
		/// <param name="transitionType"></param>
		/// <param name="delay"></param>
		/// <param name="inBetweenAction"></param>
#nullable enable
		public void Transition(TransitionType transitionType, TransitionPlayType? transitionPlayType = TransitionPlayType.All,
							   float? delay = 0, Action? inBetweenAction = default)
		{
			if (isRunningTransition) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, transitionPlayType ?? TransitionPlayType.All, delay ?? 0, transitionSettings, inBetweenAction ?? default).Forget();
		}

		public void TransitionFunc(TransitionType transitionType, TransitionPlayType? transitionPlayType = TransitionPlayType.All,
							   float? delay = 0, Func<UniTask<bool>>? inBetweenFunc = default)
		{
			if (isRunningTransition) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, transitionPlayType ?? TransitionPlayType.All, delay ?? 0, transitionSettings, inBetweenFunc ?? default).Forget();
		}
#nullable disable

		/// <summary>
		/// The first fade in will appear when you start the game.
		/// Rather than using addressable to spawn it, which will take a few frames, the pre-defined prefab is already inside.
		/// Register it onto the dictionary.
		/// </summary>
		void InitialSetup()
		{
			TransitionType transitionType = TransitionType.Fade;
			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);

			Transition transition = GetComponentInChildren<Transition>();
			transitionDictionary.Add(transitionType, transition);
		}

		async UniTask HandleTransition(TransitionType transitionType, TransitionPlayType transitionPlayType, float delay,
									   TransitionSettings transitionSettings, Action inBetweenAction)
		{
			Transition transition = await GetTransition(transitionType, delay);
			await TransitionSetup(transition, transitionPlayType, transitionSettings, inBetweenAction);

			isRunningTransition = false;
		}

		async UniTask HandleTransition(TransitionType transitionType, TransitionPlayType transitionPlayType, float delay,
									   TransitionSettings transitionSettings, Func<UniTask<bool>> inBetweenFunc)
		{
			Transition transition = await GetTransition(transitionType, delay);
			await TransitionSetupFunc(transition, transitionPlayType, transitionSettings, inBetweenFunc);

			isRunningTransition = false;
		}

		async UniTask<Transition> GetTransition(TransitionType transitionType, float delay)
		{
			isRunningTransition = true;
			await UniTask.Delay(delay.SecondsToMilliseconds());

			if (!transitionDictionary.TryGetValue(transitionType, out Transition transition))
				transition = await SpawnTemplate(transitionType);

			return transition;
		}

		async UniTask<Transition> SpawnTemplate(TransitionType transitionType)
		{
			GameObject template = await AddressableHelper.Spawn(transitionType.GetStringValue(), Vector3.zero, CanvasGroup.transform);

			Transition transition = template.GetComponent<Transition>();
			transitionDictionary.Add(transitionType, transition);

			return transition;
		}

		async UniTask TransitionSetup(Transition transition, TransitionPlayType transitionPlayType, TransitionSettings transitionSettings, Action inBetweenAction)
		{
			CanvasSetup(transitionSettings);
			await transition.Begin(transitionSettings, transitionPlayType, transitionManagerSettings, inBetweenAction);
		}

		async UniTask TransitionSetupFunc(Transition transition, TransitionPlayType transitionPlayType, TransitionSettings transitionSettings, Func<UniTask<bool>> inBetweenFunc)
		{
			CanvasSetup(transitionSettings);
			await transition.Begin(transitionSettings, transitionPlayType, transitionManagerSettings, inBetweenFunc);
		}

		void CanvasSetup(TransitionSettings transitionSettings)
		{
			// Canvas setup.
			canvasScaler.referenceResolution = transitionSettings.ReferenceResolution;
			canvasGroup.blocksRaycasts = transitionSettings.BlockRaycasts;
		}
	}
}
