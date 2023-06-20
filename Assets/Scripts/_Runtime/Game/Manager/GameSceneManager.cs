using System;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Transition;
using Personal.Constant;
using Helper;

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

		public void ChangeLevel(int index, TransitionType transitionType = TransitionType.Fade, Action inBetweenAction = default, float delay = 0)
		{
			Action action = () => DoAction(inBetweenAction, () => SceneManager.LoadScene(index));
			TransitionManager.Instance.Transition(transitionType, TransitionPlayType.All, delay, action);
		}

		public void ChangeLevel(string sceneName, TransitionType transitionType = TransitionType.Fade, Action inBetweenAction = default, float delay = 0)
		{
			Action action = () => DoAction(inBetweenAction, () => SceneManager.LoadScene(sceneName));
			TransitionManager.Instance.Transition(transitionType, TransitionPlayType.All, delay, action);
		}

		public void ChangeLevelFunc(int index, TransitionType transitionType = TransitionType.Fade, Func<UniTask<bool>> inBetweenFunc = default,
									float delay = 0, float delayAfter = 0)
		{
			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(inBetweenFunc, () => SceneManager.LoadScene(index), delayAfter);
				return true;
			};
			TransitionManager.Instance.TransitionFunc(transitionType, TransitionPlayType.All, delay, func);
		}

		public void ChangeLevelFunc(string sceneName, TransitionType transitionType = TransitionType.Fade, Func<UniTask<bool>> inBetweenFunc = default,
									float delay = 0, float delayAfter = 0)
		{
			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(inBetweenFunc, () => SceneManager.LoadScene(sceneName), delayAfter);
				return true;
			};
			TransitionManager.Instance.TransitionFunc(transitionType, TransitionPlayType.All, delay, func);
		}

		void DoAction(Action inBetweenAction, Action doLast)
		{
			inBetweenAction?.Invoke();
			doLast?.Invoke();
		}

		async UniTask DoFunc(Func<UniTask<bool>> inBetweenFunc, Action action, float delayAfter)
		{
			await inBetweenFunc();
			action?.Invoke();
			await UniTask.Delay(delayAfter.SecondsToMilliseconds());
		}
	}
}

