using UnityEngine;
using System.Collections.Generic;

using Helper;
using Personal.System.Handler;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class SceneManager : MonoBehaviourSingleton<SceneManager>
	{
		[SerializeField] FadeHandler fadeHandler = null;

		public void ChangeLevel(int index)
		{
			fadeHandler.FadeOutLoadScene(index).Forget();
		}
	}
}

