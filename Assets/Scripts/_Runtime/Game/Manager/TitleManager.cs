using UnityEngine;

using Personal.GameState;
using Personal.Transition;

namespace Personal.Manager
{
	public class TitleManager : GameInitializeSingleton<TitleManager>
	{
		public void MainScene()
		{
			GameSceneManager.Instance.ChangeLevel(SceneName.Main, TransitionType.Fade);
		}
	}
}

