using UnityEngine;

using Helper;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class CursorHandler : GameInitialize
	{
		protected override async UniTask Awake()
		{
			await base.Awake();

			CoroutineHelper.RunActionUntilBreak(0, () => transform.position = Input.mousePosition, default);
		}
	}
}

