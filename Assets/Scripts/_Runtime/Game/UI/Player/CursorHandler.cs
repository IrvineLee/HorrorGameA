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
			float x = (Screen.width / 2);// - (cursorImage.sprite.bounds.size.x / 2);
			float y = (Screen.height / 2);// - (cursorImage.sprite.bounds.size.y / 2);

			// Set the cursor to the selected response or center of the screen.
			Vector2 screenPosition = dialogueSetup.IsWaitingResponse ? EventSystem.current.currentSelectedGameObject.transform.position : new Vector2(x, y);
			Mouse.current.WarpCursorPosition(screenPosition);
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

