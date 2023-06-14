using UnityEngine;
using UnityEngine.SceneManagement;

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
			if (SceneManager.GetActiveScene().name.Equals(SceneName.Title))
			{
				SetToMouseCursor(true);
				return;
			}

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