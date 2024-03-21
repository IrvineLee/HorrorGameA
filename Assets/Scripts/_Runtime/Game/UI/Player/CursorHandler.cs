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
		[SerializeField] Transform mouseTrans = null;
		[SerializeField] Image cursorImage = null;
		[SerializeField] Image blockerImage = null;

		void LateUpdate()
		{
			mouseTrans.position = Input.mousePosition;
		}

		public void EnableCursor(bool isFlag)
		{
			mouseTrans.gameObject.SetActive(isFlag);
			blockerImage.gameObject.SetActive(!isFlag);
		}

		public void SetToCurrentSelectedGO()
		{
			if (!EventSystem.current.currentSelectedGameObject) return;

			// Wait for the LateUpdate to finish first.
			CoroutineHelper.WaitNextFrame(() =>
			{
				Transform selectedTrans = EventSystem.current.currentSelectedGameObject.transform;
				mouseTrans.position = selectedTrans.position;

				Mouse.current.WarpCursorPosition(selectedTrans.position);
			}, isEndOfFrame: true);
		}

		public void SetImage(Sprite sprite)
		{
			cursorImage.sprite = sprite;
		}
	}
}