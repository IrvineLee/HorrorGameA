using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using PixelCrushers;
using Personal.GameState;
using Personal.Definition;
using Personal.UI;
using Personal.Dialogue;

namespace Personal.Manager
{
	public class CursorManager : GameInitializeSingleton<CursorManager>
	{
		[SerializeField] CursorHandler mouseCursorHandler = null;

		[Space]
		[SerializeField] CursorDefinition cursorDefinition = null;
		[SerializeField] Image centerCrosshairImage = null;

		bool IsCanChangeToMouse
		{
			get => GameSceneManager.Instance.IsScene(SceneName.Title) || dialogueSetup.IsWaitingResponse ||
				  (!UIManager.IsWindowStackEmpty && UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Dialogue) ||
				  InputManager.Instance.CurrentActionMapType == InputProcessing.ActionMapType.Puzzle;
		}

		CursorDefinition.CrosshairType currentCrosshairType = CursorDefinition.CrosshairType.FPS;
		CursorDefinition.CrosshairType defaultCrosshairType = CursorDefinition.CrosshairType.FPS;

		DialogueSetup dialogueSetup;

		protected override void Initialize()
		{
			dialogueSetup = StageManager.Instance.DialogueController.DialogueSetup;

			cursorDefinition.Initialize();
			mouseCursorHandler.SetImage(cursorDefinition.MouseCursor);

			InputManager.OnDeviceIconChanged += HandleCursorAndMouseChange;
		}

		protected override void OnTitleScene()
		{
			HandleCursorAndMouseChange();
			SetCenterCrosshairSprite(CursorDefinition.CrosshairType.UI_Nothing);
		}

		protected override void OnEarlyMainScene()
		{
			SetToMouseCursor(false);
			SetCenterCrosshairToDefault();
		}

		/// <summary>
		/// This changes the control from gamepad to mouse and vice-versa.
		/// </summary>
		/// <param name="isFlag"></param>
		public void SetToMouseCursor(bool isFlag)
		{
			InputDeviceManager.instance.SetCursorConfined();

			centerCrosshairImage.gameObject.SetActive(!isFlag);
			mouseCursorHandler.gameObject.SetActive(isFlag);
		}

		/// <summary>
		/// This checks whether the user is using mouse first before setting it to mouse cursor.
		/// </summary>
		public void TrySetToMouseCursorForMouseControl(bool isFlag, bool isCrosshairNothing = false)
		{
			if (isCrosshairNothing)
			{
				SetCenterCrosshairSprite(CursorDefinition.CrosshairType.UI_Nothing);
			}
			else
			{
				SetCenterCrosshairToDefault();
			}

			if (!InputManager.Instance.IsCurrentDeviceMouse) return;
			SetToMouseCursor(isFlag);
		}

		/// <summary>
		/// This is the center crosshair when in FPS mode, NOT when the mouse cursor is enabled,
		/// </summary>
		/// <param name="crosshairType"></param>
		public void SetCenterCrosshair(CursorDefinition.CrosshairType crosshairType)
		{
			SetCenterCrosshairSprite(crosshairType);
		}

		/// <summary>
		/// Set the center crosshair back to it's default state.
		/// </summary>
		public void SetCenterCrosshairToDefault()
		{
			SetCenterCrosshairSprite(defaultCrosshairType);
		}

		void SetCenterCrosshairSprite(CursorDefinition.CrosshairType compareCrosshair)
		{
			if (currentCrosshairType == compareCrosshair) return;

			cursorDefinition.CrosshairDictionary.TryGetValue(compareCrosshair, out Sprite sprite);

			currentCrosshairType = compareCrosshair;
			centerCrosshairImage.sprite = sprite;
			centerCrosshairImage.enabled = true;

			if (compareCrosshair == CursorDefinition.CrosshairType.UI_Nothing)
			{
				centerCrosshairImage.enabled = false;
			}
		}

		void HandleCursorAndMouseChange()
		{
			// You don't want to reset the mouse cursor when the user changed the icon only. This only happens in option.
			if (InputManager.Instance.IsChangeIconOnly) return;

			// Handle the dialogue response.
			if (dialogueSetup.IsWaitingResponse)
			{
				Vector2 screenPosition = Vector2.zero;
				if (InputManager.Instance.IsCurrentDeviceMouse && EventSystem.current.currentSelectedGameObject)
				{
					screenPosition = EventSystem.current.currentSelectedGameObject.transform.position;
				}
				Mouse.current.WarpCursorPosition(screenPosition);
			}

			Cursor.lockState = CursorLockMode.Confined;

			if (InputManager.Instance.IsCurrentDeviceMouse && IsCanChangeToMouse)
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
	}
}