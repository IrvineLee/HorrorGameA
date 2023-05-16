using UnityEngine;
using UnityEngine.UI;

using Helper;
using Personal.Setting.Audio;
using Personal.Definition;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.UI
{
	public class CursorHandler : GameInitialize
	{
		Image image;

		protected override async UniTask Awake()
		{
			await base.Awake();

			image = GetComponentInChildren<Image>();

			CoroutineHelper.RunActionUntilBreak(0, () =>
			{
				Cursor.visible = false;
				transform.position = Input.mousePosition;
			}, default);
		}
	}
}

