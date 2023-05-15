using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Personal.GameState;
using EasyTransition;

namespace Personal.Manager
{
	public class GameSceneManager : GameInitializeSingleton<GameSceneManager>
	{
		public void ChangeLevel(int index, TransitionType transitionType = TransitionType.Fade, Action inBetweenAction = default, float delay = 0)
		{
			Action action = () =>
			{
				inBetweenAction?.Invoke();
				SceneManager.LoadScene(index);
			};
			TransitionManager.Instance.Transition(transitionType, TransitionPlayType.All, delay, action);
		}

		public void ChangeLevel(string sceneName, TransitionType transitionType = TransitionType.Fade, Action inBetweenAction = default, float delay = 0)
		{
			Action action = () =>
			{
				inBetweenAction?.Invoke();
				SceneManager.LoadScene(sceneName);
			};
			TransitionManager.Instance.Transition(transitionType, TransitionPlayType.All, delay, action);
		}
	}
}

