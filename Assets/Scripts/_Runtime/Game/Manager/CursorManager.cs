using UnityEngine;

using PixelCrushers;
using Personal.GameState;

namespace Personal.Manager
{
	public class CursorManager : GameInitializeSingleton<CursorManager>
	{
		[SerializeField] Transform crosshairUI = null;
		[SerializeField] Transform mouseCursorUI = null;

		protected override void Initialize()
		{
			SetToMouseCursor(true);
		}

		protected override void OnEarlyMainScene()
		{
			SetToMouseCursor(false);
		}

		public void SetToMouseCursor(bool isFlag)
		{
			Cursor.visible = false;
			InputDeviceManager.instance.ForceCursorFalse();

			crosshairUI.gameObject.SetActive(!isFlag);
			mouseCursorUI.gameObject.SetActive(isFlag);
		}

		void OnApplicationFocus(bool hasFocus)
		{
			SetToMouseCursor(!hasFocus);
		}
	}
}