using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.GameState;

namespace Personal.InteractiveObject
{
	public class MainSceneHandlerBase : GameInitialize
	{
		protected override async UniTask OnMainSceneNext()
		{
			await HandleStartScene();
		}

		protected virtual UniTask HandleStartScene() { return UniTask.CompletedTask; }
	}
}

