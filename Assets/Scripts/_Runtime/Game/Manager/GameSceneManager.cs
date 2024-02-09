using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.GameState;
using Personal.Transition;

namespace Personal.Manager
{
	public class GameSceneManager : GameInitializeSingleton<GameSceneManager>
	{
		[SerializeField] string mainScenePath = "Assets/Resources/MainScenes";
		public string CurrentSceneName { get => SceneManager.GetActiveScene().name; }

		List<string> mainSceneList = new();

		public void Init()
		{
			DirectoryInfo dir = new DirectoryInfo(mainScenePath);
			var fileArray = dir.GetFiles("*.unity");

			foreach (var file in fileArray)
			{
				string sceneName = file.Name.SearchBehindRemoveFrontOrEnd('.', true);
				mainSceneList.Add(sceneName);
				Debug.Log(sceneName);
			}
		}

		public bool IsMainScene()
		{
			foreach (var scene in mainSceneList)
			{
				if (!string.Equals(scene, CurrentSceneName)) continue;
				return true;
			}
			return false;
		}

		public bool IsScene(string sceneName)
		{
			if (string.Equals(sceneName, CurrentSceneName)) return true;
			return false;
		}

		/// <summary>
		/// Change the level based on the index.
		/// </summary>
		public void ChangeLevel(int index, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
								Action inBetweenAction = default, float delay = 0, bool isIgnoreTimescale = false, Action newSceneAction = default)
		{
			UIManager.Instance.ToolsUI.BlockInput(true);

			Action action = () => DoAction(inBetweenAction, () => SceneManager.LoadScene(index), () => BundleNewSceneAction(newSceneAction));
			TransitionManager.Instance.Transition(transitionType, transitionPlayType, delay, action, isIgnoreTimescale);
		}

		/// <summary>
		/// Change the level based on string name.
		/// </summary>
		public void ChangeLevel(string sceneName, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
								Action endTransitionAction = default, float delay = 0, bool isIgnoreTimescale = false, Action newSceneAction = default)
		{
			UIManager.Instance.ToolsUI.BlockInput(true);

			Action action = () => DoAction(endTransitionAction, () => SceneManager.LoadScene(sceneName), () => BundleNewSceneAction(newSceneAction));
			TransitionManager.Instance.Transition(transitionType, transitionPlayType, delay, action, isIgnoreTimescale);
		}

		public void ChangeLevelFunc(int index, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
									Func<UniTask<bool>> endTransitionFunc = default, float delay = 0, bool isIgnoreTimescale = false, Action newSceneAction = default)
		{
			UIManager.Instance.ToolsUI.BlockInput(true);

			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(endTransitionFunc, () => SceneManager.LoadScene(index), () => BundleNewSceneAction(newSceneAction));
				return true;
			};
			TransitionManager.Instance.TransitionFunc(transitionType, transitionPlayType, delay, func, isIgnoreTimescale);
		}

		public void ChangeLevelFunc(string sceneName, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
									Func<UniTask<bool>> endTransitionFunc = default, float delay = 0, bool isIgnoreTimescale = false, Action newSceneAction = default)
		{
			UIManager.Instance.ToolsUI.BlockInput(true);

			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(endTransitionFunc, () => SceneManager.LoadScene(sceneName), () => BundleNewSceneAction(newSceneAction));
				return true;
			};
			TransitionManager.Instance.TransitionFunc(transitionType, transitionPlayType, delay, func, isIgnoreTimescale);
		}

		/// <summary>
		/// When you reached the new scene, disable the block input.
		/// </summary>
		/// <param name="newSceneAction"></param>
		void BundleNewSceneAction(Action newSceneAction)
		{
			newSceneAction?.Invoke();
			UIManager.Instance.ToolsUI.BlockInput(false);
		}

		void DoAction(Action endTransitionAction, Action loadSceneAction, Action newSceneAction)
		{
			endTransitionAction?.Invoke();
			CoroutineHelper.WaitEndOfFrame(() => loadSceneAction?.Invoke());
			CoroutineHelper.WaitNextFrame(newSceneAction);
		}

		async UniTask DoFunc(Func<UniTask<bool>> endTransitionFunc, Action loadSceneAction, Action newSceneAction)
		{
			await endTransitionFunc();
			CoroutineHelper.WaitEndOfFrame(() => loadSceneAction?.Invoke());
			CoroutineHelper.WaitNextFrame(newSceneAction);
		}
	}
}

