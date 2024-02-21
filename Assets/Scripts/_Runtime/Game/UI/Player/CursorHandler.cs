using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Helper;
using Personal.GameState;

namespace Personal.UI
{
	public class CursorHandler : GameInitialize
	{
		[SerializeField] Image cursorImage = null;

		void LateUpdate()
		{
			transform.position = Input.mousePosition;
		}

		public void SetToCurrentSelectedGO()
		{
			if (!EventSystem.current.currentSelectedGameObject) return;

			// Wait for the LateUpdate to finish first.
			CoroutineHelper.WaitNextFrame(() =>
			{
				Transform trans = EventSystem.current.currentSelectedGameObject.transform;
				transform.position = trans.position;

				Mouse.current.WarpCursorPosition(trans.position);
			}, isEndOfFrame: true);
		}

		public void SetImage(Sprite sprite)
		{
			cursorImage.sprite = sprite;
		}
	}
}