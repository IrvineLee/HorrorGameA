using UnityEngine;

using Personal.GameState;
using UnityEngine.UI;

namespace Personal.UI
{
	public class CursorHandler : GameInitialize
	{
		[SerializeField] Image cursorImage = null;

		protected override void OnUpdate()
		{
			transform.position = Input.mousePosition;
		}

		public void SetImage(Sprite sprite)
		{
			cursorImage.sprite = sprite;
		}
	}
}

