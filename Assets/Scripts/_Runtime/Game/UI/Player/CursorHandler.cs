using UnityEngine;

using Helper;
using Personal.GameState;

namespace Personal.UI
{
	public class CursorHandler : GameInitialize
	{
		protected override void Initialize()
		{
			CoroutineHelper.RunActionUntilBreak(0, () => transform.position = Input.mousePosition, default);
		}
	}
}

