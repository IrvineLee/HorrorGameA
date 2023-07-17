using UnityEngine;
using UnityEngine.EventSystems;

using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.Dialogue
{
	public class DialogueSetup : MonoBehaviour
	{
		[SerializeField] ActionMapType actionMapType = ActionMapType.BasicControl;

		public bool IsWaitingResponse { get => isWaitingResponse; }

		ActionMapType previousActionMap;

		StandardUIMenuPanel standardUIMenuPanel;
		bool isWaitingResponse;

		void Awake()
		{
			var dialogueSystemController = GetComponentInChildren<DialogueSystemController>(true);
			GameObject dialogueUI = dialogueSystemController.displaySettings.dialogueUI;
			standardUIMenuPanel = dialogueUI.GetComponentInChildren<StandardDialogueUI>(true).conversationUIElements.defaultMenuPanel;

			// For the response window.
			standardUIMenuPanel.onOpen.AddListener(() =>
			{
				isWaitingResponse = true;
				HandleCursor();

				if (!InputManager.Instance.IsCurrentDeviceMouse) return;
				CursorManager.Instance.SetToMouseCursor(true);
			});

			standardUIMenuPanel.onClose.AddListener(() =>
			{
				isWaitingResponse = false;

				if (!InputManager.Instance.IsCurrentDeviceMouse) return;
				CursorManager.Instance.SetToMouseCursor(false);
			});

			InputManager.OnDeviceIconChanged += HandleCursor;
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationStart(Transform actor)
		{
			previousActionMap = InputManager.Instance.CurrentActionMapType;
			InputManager.Instance.EnableActionMap(actionMapType);
		}

		/// <summary>
		/// Called by DialogueManager.
		/// </summary>
		/// <param name="actor"></param>
		void OnConversationEnd(Transform actor)
		{
			InputManager.Instance.EnableActionMap(previousActionMap);
			isWaitingResponse = false;
			InputDeviceManager.instance.alwaysAutoFocus = false;
		}

		void HandleCursor()
		{
			if (!GameSceneManager.Instance.IsMainScene()) return;
			if (UIManager.Instance.ActiveInterfaceType != UI.UIInterfaceType.Dialogue) return;

			if (InputManager.Instance.IsCurrentDeviceMouse && isWaitingResponse)
			{
				ResponseFocus(false);
				EventSystem.current.SetSelectedGameObject(null);

				return;
			}
			ResponseFocus(true);
		}

		void ResponseFocus(bool isFlag)
		{
			InputDeviceManager.instance.alwaysAutoFocus = isFlag;
			standardUIMenuPanel.focusCheckFrequency = isFlag ? 0.1f : 0;
		}

		void OnDisable()
		{
			standardUIMenuPanel.onOpen.RemoveAllListeners();
			standardUIMenuPanel.onClose.RemoveAllListeners();

			InputManager.OnDeviceIconChanged -= HandleCursor;
		}
	}
}