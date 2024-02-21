using UnityEngine;
using UnityEngine.UI;

using Helper;
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
			get => InputManager.Instance.IsCurrentDeviceMouse &&
				(GameSceneManager.Instance.IsScene(SceneName.Title) || dialogueSetup.IsWaitingResponse ||
				(!UIManager.IsWindowStackEmpty && UIManager.Instance.ActiveInterfaceType != UIInterfaceType.Dialogue) ||
				InputManager.Instance.CurrentActionMapType == InputProcessing.ActionMapType.Puzzle);
		}

		bool IsFPSMode
		{
			get => GameSceneManager.Instance.IsMainScene() &&
				InputManager.Instance.CurrentActionMapType == InputProcessing.ActionMapType.Player &&
				UIManager.Instance.ActiveInterfaceType == UIInterfaceType.None;
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
		/// This changes the control from gamepad to mouse and vice-versa if gamepad/mouse is being used.
		/// Might enable/disable mouse after it completes.
		/// </summary>
		public void HandleMouse(bool isSetToSelectedGO = true)
		{
			Cursor.visible = false;

			bool isFPSMode = IsFPSMode;
			bool isShowMouseCursor = IsCanChangeToMouse && !isFPSMode;

			//Debug.Log("isFPSMode " + isFPSMode + " IsCanChangeToMouse " + IsCanChangeToMouse + " isShowMouseCursor " + isShowMouseCursor);

			Cursor.lockState = CursorLockMode.Confined;
			if (isFPSMode) Cursor.lockState = CursorLockMode.Locked;

			if (isShowMouseCursor && isSetToSelectedGO)
			{
				mouseCursorHandler.SetToCurrentSelectedGO();
			}

			CoroutineHelper.WaitNextFrame(() =>
			{
				mouseCursorHandler.gameObject.SetActive(isShowMouseCursor);
				SetCenterCrosshair(isFPSMode ? CursorDefinition.CrosshairType.FPS : CursorDefinition.CrosshairType.Nothing);
			}, isEndOfFrame: true);
		}

		/// <summary>
		/// Disable the mouse cursor. Sometime you might wanna disable the mouse earlier. Ex: When a window starts closing/Puzzle is over etc.
		/// </summary>
		public void HideMouseCursor()
		{
			Cursor.visible = false;

			mouseCursorHandler.gameObject.SetActive(false);
			if (GameSceneManager.Instance.IsMainScene()) SetCenterCrosshair(CursorDefinition.CrosshairType.FPS);
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

			HandleMouse();
		}

		void OnApplicationFocus(bool hasFocus)
		{
			if (!Application.isEditor) HandleMouse();
		}

		void OnApplicationQuit()
		{
			InputManager.OnDeviceIconChanged -= HandleCursorAndMouseChange;
		}
	}
}