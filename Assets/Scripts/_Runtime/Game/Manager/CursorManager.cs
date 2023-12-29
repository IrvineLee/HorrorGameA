using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

		CursorDefinition.CrosshairType currentCrosshairType = CursorDefinition.CrosshairType.Nothing;

		DialogueSetup dialogueSetup;

		protected override void Initialize()
		{
			dialogueSetup = StageManager.Instance.DialogueController.DialogueSetup;

			cursorDefinition.Initialize();
			mouseCursorHandler.SetImage(cursorDefinition.MouseCursor);

			Cursor.visible = false;
			InputManager.OnDeviceIconChanged += HandleCursorAndMouseChange;
		}

		protected override void OnTitleScene()
		{
			HandleCursorAndMouseChange();
			SetCenterCrosshair(CursorDefinition.CrosshairType.Nothing);
		}

		protected override void OnEarlyMainScene()
		{
			HandleCursorAndMouseChange();
			SetCenterCrosshair(CursorDefinition.CrosshairType.FPS);
		}

		/// <summary>
		/// This changes the control from gamepad to mouse and vice-versa.
		/// </summary>
		/// <param name="isFlag"></param>
		/// <param name="isOnlyActiveIfMouse">Whether to only activate the mouse when you are currently using mouse</param>
		/// <param name="crosshairType"></param>
		public void SetToMouseCursor(bool isFlag, bool isOnlyActiveIfMouse = false, CursorDefinition.CrosshairType crosshairType = CursorDefinition.CrosshairType.FPS)
		{
			Cursor.visible = false;

			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				Cursor.lockState = CursorLockMode.Confined;
			}

			if (InputManager.Instance.CurrentActionMapType == InputProcessing.ActionMapType.Player)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}

			SetCenterCrosshair(crosshairType);

			if (!isOnlyActiveIfMouse ||
				(isOnlyActiveIfMouse && InputManager.Instance.IsCurrentDeviceMouse))
			{
				mouseCursorHandler.gameObject.SetActive(isFlag);
			}
		}

		/// <summary>
		/// This is the center crosshair when in FPS mode, NOT when the mouse cursor is enabled,
		/// </summary>
		/// <param name="crosshairType"></param>
		public void SetCenterCrosshair(CursorDefinition.CrosshairType crosshairType)
		{
			if (currentCrosshairType == crosshairType) return;

			cursorDefinition.CrosshairDictionary.TryGetValue(crosshairType, out Sprite sprite);

			currentCrosshairType = crosshairType;
			centerCrosshairImage.sprite = sprite;
			centerCrosshairImage.gameObject.SetActive(crosshairType != CursorDefinition.CrosshairType.Nothing);
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

			SetToMouseCursor(IsCanChangeToMouse, true);
		}

		void OnApplicationQuit()
		{
			InputManager.OnDeviceIconChanged -= HandleCursorAndMouseChange;
		}
	}
}