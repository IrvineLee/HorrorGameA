using UnityEngine;
using UnityEngine.UI;

using PixelCrushers;
using Personal.GameState;
using Personal.Definition;
using Personal.UI;

namespace Personal.Manager
{
	public class CursorManager : GameInitializeSingleton<CursorManager>
	{
		[SerializeField] CursorHandler mouseCursorHandler = null;

		[Space]
		[SerializeField] CursorDefinition cursorDefinition = null;
		[SerializeField] Image crosshairImage = null;

		CursorDefinition.CrosshairType currentCrosshairType = CursorDefinition.CrosshairType.FPS;

		protected override void Initialize()
		{
			cursorDefinition.Initialize();
			mouseCursorHandler.SetImage(cursorDefinition.MouseCursor);

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

			crosshairImage.gameObject.SetActive(!isFlag);
			mouseCursorHandler.gameObject.SetActive(isFlag);
		}

		public void SetCrosshair(CursorDefinition.CrosshairType crosshairType)
		{
			if (currentCrosshairType == crosshairType) return;

			cursorDefinition.CrosshairDictionary.TryGetValue(crosshairType, out Sprite sprite);

			currentCrosshairType = crosshairType;
			crosshairImage.sprite = sprite;
		}

		public void SetToDefaultCrosshair()
		{
			cursorDefinition.CrosshairDictionary.TryGetValue(CursorDefinition.CrosshairType.FPS, out Sprite sprite);

			currentCrosshairType = CursorDefinition.CrosshairType.FPS;
			crosshairImage.sprite = sprite;
		}

		void OnApplicationFocus(bool hasFocus)
		{
			SetToMouseCursor(!hasFocus);
		}
	}
}