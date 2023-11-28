using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Personal.Dialogue;
using Personal.Manager;
using Personal.GameState;

namespace Personal.UI
{
	public class CursorHandler : GameInitialize
	{
		[SerializeField] Image cursorImage = null;

		DialogueSetup dialogueSetup;

		protected override void EarlyInitialize()
		{
			dialogueSetup = StageManager.Instance.DialogueController.DialogueSetup;
		}

		void OnEnable()
		{
			if (!dialogueSetup.IsWaitingResponse) return;

			// Set the cursor to the selected response or center of the screen.
			Transform trans = EventSystem.current.currentSelectedGameObject.transform;
			Mouse.current.WarpCursorPosition(trans.position);
		}

		void Update()
		{
			transform.position = Input.mousePosition;
		}

		public void SetImage(Sprite sprite)
		{
			cursorImage.sprite = sprite;
		}
	}
}

