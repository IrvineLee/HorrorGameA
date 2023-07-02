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

		protected override void OnTitleScene()
		{
			SetToMouseCursor(true);
		}

		protected override void OnEarlyMainScene()
		{
			SetToMouseCursor(false);
		}

		/// <summary>
		/// This only works for mouse input, NOT gamepad.
		/// </summary>
		/// <param name="isFlag"></param>
		public void SetToMouseCursor(bool isFlag)
		{
			InputDeviceManager.instance.ForceCursorFalse();

			crosshairImage.gameObject.SetActive(!isFlag);
			mouseCursorHandler.gameObject.SetActive(isFlag);
		}

		/// <summary>
		/// This is the crosshair when in FPS mode, NOT when the mouse cursor is enabled,
		/// </summary>
		/// <param name="crosshairType"></param>
		public void SetCrosshair(CursorDefinition.CrosshairType crosshairType)
		{
			if (currentCrosshairType == crosshairType) return;

			cursorDefinition.CrosshairDictionary.TryGetValue(crosshairType, out Sprite sprite);

			currentCrosshairType = crosshairType;
			crosshairImage.sprite = sprite;
		}

		/// <summary>
		/// Set the crosshair back to it's default state.
		/// </summary>
		public void SetToDefaultCrosshair()
		{
			cursorDefinition.CrosshairDictionary.TryGetValue(CursorDefinition.CrosshairType.FPS, out Sprite sprite);

			currentCrosshairType = CursorDefinition.CrosshairType.FPS;
			crosshairImage.sprite = sprite;
		}

		void OnApplicationFocus(bool hasFocus)
		{
			if (GameSceneManager.Instance.IsMainScene() && UIManager.Instance.IsWindowStackEmpty)
			{
				SetToMouseCursor(false);
				return;
			}
			SetToMouseCursor(true);
		}
	}
}