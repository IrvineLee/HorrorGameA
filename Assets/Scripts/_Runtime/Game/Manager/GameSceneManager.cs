using System;
using UnityEngine.SceneManagement;

using Personal.GameState;
using Personal.Transition;
using Personal.Constant;

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

