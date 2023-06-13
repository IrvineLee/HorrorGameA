using UnityEngine;

using Personal.GameState;

namespace Personal.Manager
{
	public class TitleManager : GameInitializeSingleton<TitleManager>
	{
		public void MainScene()
		{
			GameSceneManager.Instance.ChangeLevel(SceneName.Main, EasyTransition.TransitionType.Fade);
		}
	}
}

