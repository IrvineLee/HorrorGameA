using System;
using System.Collections.Generic;
using System.Linq;
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
		public bool IsTransitioning { get => isTransitioning; }

		CanvasGroup canvasGroup;
		bool isTransitioning;

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
							   float? delay = 0, Action? inBetweenAction = default, bool isIgnoreTimescale = false)
		{
			if (isTransitioning) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, transitionPlayType ?? TransitionPlayType.All, delay ?? 0, transitionSettings, inBetweenAction ?? default, isIgnoreTimescale).Forget();
		}

		public void TransitionFunc(TransitionType transitionType, TransitionPlayType? transitionPlayType = TransitionPlayType.All,
							   float? delay = 0, Func<UniTask<bool>>? inBetweenFunc = default, bool isIgnoreTimescale = false)
		{
			if (isTransitioning) return;

			TransitionSettings transitionSettings = TransitionManagerSettings.GetTransitionSetting(transitionType);
			HandleTransition(transitionType, transitionPlayType ?? TransitionPlayType.All, delay ?? 0, transitionSettings, inBetweenFunc ?? default, isIgnoreTimescale).Forget();
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
									   TransitionSettings transitionSettings, Action inBetweenAction, bool isIgnoreTimescale)
		{
			Transition transition = await GetTransition(transitionType, delay, isIgnoreTimescale);
			await TransitionSetup(transition, transitionPlayType, transitionSettings, inBetweenAction, isIgnoreTimescale);

			isTransitioning = false;
		}

		async UniTask HandleTransition(TransitionType transitionType, TransitionPlayType transitionPlayType, float delay,
									   TransitionSettings transitionSettings, Func<UniTask<bool>> inBetweenFunc, bool isIgnoreTimescale)
		{
			Transition transition = await GetTransition(transitionType, delay, isIgnoreTimescale);
			await TransitionSetupFunc(transition, transitionPlayType, transitionSettings, inBetweenFunc, isIgnoreTimescale);

			isTransitioning = false;
		}

		async UniTask<Transition> GetTransition(TransitionType transitionType, float delay, bool isIgnoreTimescale)
		{
			isTransitioning = true;
			await UniTask.Delay(delay.SecondsToMilliseconds(), isIgnoreTimescale);

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

		async UniTask TransitionSetup(Transition transition, TransitionPlayType transitionPlayType, TransitionSettings transitionSettings,
									Action inBetweenAction, bool isIgnoreTimescale)
		{
			CanvasSetup(transitionSettings);
			await transition.Begin(transitionSettings, transitionPlayType, transitionManagerSettings, inBetweenAction, isIgnoreTimescale);
		}

		async UniTask TransitionSetupFunc(Transition transition, TransitionPlayType transitionPlayType, TransitionSettings transitionSettings,
										Func<UniTask<bool>> inBetweenFunc, bool isIgnoreTimescale)
		{
			CanvasSetup(transitionSettings);
			await transition.Begin(transitionSettings, transitionPlayType, transitionManagerSettings, inBetweenFunc, isIgnoreTimescale);
		}

		void CanvasSetup(TransitionSettings transitionSettings)
		{
			// Canvas setup.
			canvasScaler.referenceResolution = transitionSettings.ReferenceResolution;
			canvasGroup.blocksRaycasts = transitionSettings.BlockRaycasts;
		}
	}
}
