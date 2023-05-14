using UnityEngine;
using System.Collections.Generic;

using Helper;
using Personal.System.Handler;
using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.Manager
{
	public class GameSceneManager : GameInitializeSingleton<GameSceneManager>
	{
		[SerializeField] FadeHandler fadeHandler = null;

		public void ChangeLevel(int index)
		{
			fadeHandler.FadeOutIn_LoadScene(index).Forget();
		}
	}
}

