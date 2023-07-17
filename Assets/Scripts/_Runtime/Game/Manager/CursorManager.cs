using UnityEngine;
using UnityEngine.UI;

using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Definition;
using Personal.UI;
using Personal.Dialogue;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class CursorManager : GameInitializeSingleton<CursorManager>
	{
		[SerializeField] CursorHandler mouseCursorHandler = null;

		[Space]
		[SerializeField] CursorDefinition cursorDefinition = null;
		[SerializeField] Image crosshairImage = null;

		CursorDefinition.CrosshairType currentCrosshairType = CursorDefinition.CrosshairType.FPS;
		CursorDefinition.CrosshairType defaultCrosshairType = CursorDefinition.CrosshairType.FPS;

		DialogueSetup dialogueSetup;

		protected override void Initialize()
		{
			dialogueSetup = DialogueManager.Instance.GetComponentInChildren<DialogueSetup>();

			cursorDefinition.Initialize();
			mouseCursorHandler.SetImage(cursorDefinition.MouseCursor);

			InputManager.OnDeviceIconChanged += HandleCursorAndMouseChange;
		}

		protected override void OnTitleScene()
		{
			HandleCursorAndMouseChange();
			SetCrosshairSprite(CursorDefinition.CrosshairType.UI_Nothing);
		}

		protected override void OnEarlyMainScene()
		{
			SetToMouseCursor(false);
			SetToDefaultCrosshair();
		}

		/// <summary>
		/// This only works for mouse input, NOT gamepad.
		/// </summary>
		/// <param name="isFlag"></param>
		public void SetToMouseCursor(bool isFlag)
		{
			InputDeviceManager.instance.SetCursorConfined();

			crosshairImage.gameObject.SetActive(!isFlag);
			mouseCursorHandler.gameObject.SetActive(isFlag);
		}

		/// <summary>
		/// This is the crosshair when in FPS mode, NOT when the mouse cursor is enabled,
		/// </summary>
		/// <param name="crosshairType"></param>
		public void SetCrosshair(CursorDefinition.CrosshairType crosshairType)
		{
			SetCrosshairSprite(crosshairType);
		}

		/// <summary>
		/// Set the crosshair back to it's default state.
		/// </summary>
		public void SetToDefaultCrosshair()
		{
			SetCrosshairSprite(defaultCrosshairType);
		}

		void SetCrosshairSprite(CursorDefinition.CrosshairType compareCrosshair)
		{
			if (currentCrosshairType == compareCrosshair) return;

			cursorDefinition.CrosshairDictionary.TryGetValue(compareCrosshair, out Sprite sprite);

			currentCrosshairType = compareCrosshair;
			crosshairImage.sprite = sprite;
			crosshairImage.enabled = true;

			if (compareCrosshair == CursorDefinition.CrosshairType.UI_Nothing)
				crosshairImage.enabled = false;
		}

		async void HandleCursorAndMouseChange()
		{
			// Reset the mouse to the center of the screen.
			Cursor.lockState = CursorLockMode.Locked;
			await UniTask.Yield(PlayerLoopTiming.LastTimeUpdate);

			Cursor.lockState = CursorLockMode.Confined;

			if (InputManager.Instance.IsCurrentDeviceMouse &&
				(!GameSceneManager.Instance.IsMainScene() || dialogueSetup.IsWaitingResponse))
			{
				SetToMouseCursor(true);
				return;
			}

			SetToMouseCursor(false);
		}

		void OnApplicationQuit()
		{
			InputManager.OnDeviceIconChanged -= HandleCursorAndMouseChange;
		}

		void OnApplicationFocus(bool hasFocus)
		{
			if (!InputManager.Instance.IsCurrentDeviceMouse) return;

			if (GameSceneManager.Instance.IsMainScene() && UIManager.IsWindowStackEmpty)
			{
				SetToMouseCursor(false);
				return;
			}
			SetToMouseCursor(true);
		}
	}
}