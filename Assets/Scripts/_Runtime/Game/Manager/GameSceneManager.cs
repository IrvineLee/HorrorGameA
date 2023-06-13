using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Personal.GameState;
using EasyTransition;

namespace Personal.Manager
{
	public class GameSceneManager : GameInitializeSingleton<GameSceneManager>
	{
		[SerializeField] List<string> mainSceneList = new List<string>();

		public bool IsMainScene()
		{
			foreach (var scene in mainSceneList)
			{
				if (!string.Equals(scene, SceneManager.GetActiveScene().name)) continue;
				return true;
			}
			return false;
		}

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

