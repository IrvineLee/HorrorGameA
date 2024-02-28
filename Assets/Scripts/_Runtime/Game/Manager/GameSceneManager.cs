using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Transition;

namespace Personal.Manager
{
	public class GameSceneManager : GameInitializeSingleton<GameSceneManager>
	{
		public string CurrentSceneName { get => SceneManager.GetActiveScene().name; }

		public bool IsMainScene { get; private set; }

		string loadingScene;

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

			Action action = () => DoAction(inBetweenAction, LoadScene(LoadSceneByIndex(index)), () => BundleNewSceneAction(newSceneAction)).Forget();
			TransitionManager.Instance.Transition(transitionType, transitionPlayType, delay, action, isIgnoreTimescale);
		}

		/// <summary>
		/// Change the level based on string name.
		/// </summary>
		public void ChangeLevel(string sceneName, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
								Action endTransitionAction = default, float delay = 0, bool isIgnoreTimescale = false, Action newSceneAction = default)
		{
			UIManager.Instance.ToolsUI.BlockInput(true);

			Action action = () => DoAction(endTransitionAction, LoadScene(LoadSceneByName(sceneName)), () => BundleNewSceneAction(newSceneAction)).Forget();
			TransitionManager.Instance.Transition(transitionType, transitionPlayType, delay, action, isIgnoreTimescale);
		}

		public void ChangeLevelFunc(int index, TransitionType transitionType = TransitionType.Fade, TransitionPlayType transitionPlayType = TransitionPlayType.All,
									Func<UniTask<bool>> endTransitionFunc = default, float delay = 0, bool isIgnoreTimescale = false, Action newSceneAction = default)
		{
			UIManager.Instance.ToolsUI.BlockInput(true);

			Func<UniTask<bool>> func = async () =>
			{
				await DoFunc(endTransitionFunc, LoadScene(LoadSceneByIndex(index)), () => BundleNewSceneAction(newSceneAction));
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
				await DoFunc(endTransitionFunc, LoadScene(LoadSceneByName(sceneName)), () => BundleNewSceneAction(newSceneAction));
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

		async UniTask DoAction(Action endTransitionAction, UniTask loadScene, Action newSceneAction)
		{
			endTransitionAction?.Invoke();

			await UniTask.NextFrame();
			await loadScene;

			newSceneAction?.Invoke();
		}

		async UniTask DoFunc(Func<UniTask<bool>> endTransitionFunc, UniTask loadScene, Action newSceneAction)
		{
			await endTransitionFunc();

			await UniTask.NextFrame();
			await loadScene;

			newSceneAction?.Invoke();
		}

		async UniTask LoadScene(AsyncOperation loadSceneFunc)
		{
			AsyncOperation asyncLoad = loadSceneFunc;

			Func<bool> func = () => asyncLoad.progress >= 0.9f;
			await UniTask.WaitUntil(func);

			IsMainScene = loadingScene.Contains("Main");
			await UniTask.WaitUntil(() => asyncLoad.isDone);
		}

		AsyncOperation LoadSceneByName(string name)
		{
			loadingScene = name;
			return SceneManager.LoadSceneAsync(name);
		}

		AsyncOperation LoadSceneByIndex(int index)
		{
			loadingScene = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
			return SceneManager.LoadSceneAsync(index);
		}
	}
}