using System;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Transition;
using Personal.Constant;
using Helper;
using UnityEngine;

namespace Personal.Manager
{
	public class GameSceneManager : GameInitializeSingleton<GameSceneManager>
	{
		public bool IsMainScene()
		{
			foreach (var scene in ConstantFixed.MAIN_SCENE_LIST)
			{
				if (!string.Equals(scene, SceneManager.GetActiveScene().name)) continue;
				return true;
			}
			return false;
		}

		public bool IsScene(string sceneName)
		{
			if (string.Equals(sceneName, SceneManager.GetActiveScene().name)) return true;
			return false;
		}

		public void ChangeLevel(int index, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
								Action inBetweenAction = default, float delay = 0, bool isIgnoreTimescale = false)
		{
			Action action = () => DoAction(inBetweenAction, () => SceneManager.LoadScene(index));
			TransitionManager.Instance.Transition(transitionType, transitionPlayType, delay, action, isIgnoreTimescale);
		}

		public void ChangeLevel(string sceneName, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
								Action inBetweenAction = default, float delay = 0, bool isIgnoreTimescale = false)
		{
			Action action = () => DoAction(inBetweenAction, () => SceneManager.LoadScene(sceneName));
			TransitionManager.Instance.Transition(transitionType, transitionPlayType, delay, action, isIgnoreTimescale);
		}

		public void ChangeLevelFunc(int index, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
									Func<UniTask<bool>> inBetweenFunc = default, float delay = 0, float delayAfter = 0, bool isIgnoreTimescale = false)
		{
			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(inBetweenFunc, () => SceneManager.LoadScene(index), delayAfter);
				return true;
			};
			TransitionManager.Instance.TransitionFunc(transitionType, transitionPlayType, delay, func, isIgnoreTimescale);
		}

		public void ChangeLevelFunc(string sceneName, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
									Func<UniTask<bool>> inBetweenFunc = default, float delay = 0, float delayAfter = 0, bool isIgnoreTimescale = false)
		{
			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(inBetweenFunc, () => SceneManager.LoadScene(sceneName), delayAfter);
				return true;
			};
			TransitionManager.Instance.TransitionFunc(transitionType, transitionPlayType, delay, func, isIgnoreTimescale);
		}

		void DoAction(Action inBetweenAction, Action doLast)
		{
			inBetweenAction?.Invoke();
			CoroutineHelper.WaitNextFrame(() => doLast?.Invoke());
		}

		async UniTask DoFunc(Func<UniTask<bool>> inBetweenFunc, Action action, float delayAfter)
		{
			await inBetweenFunc();
			action?.Invoke();
			await UniTask.Delay(delayAfter.SecondsToMilliseconds());
		}
	}
}

