using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Personal.GameState;
using Helper;

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

			Transform trans = EventSystem.current.currentSelectedGameObject.transform;
			Mouse.current.WarpCursorPosition(trans.position);

			CoroutineHelper.WaitEndOfFrame(() => transform.position = trans.position);
		}

		public void SetImage(Sprite sprite)
		{
			cursorImage.sprite = sprite;
		}
	}
}