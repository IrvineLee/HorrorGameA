using UnityEngine;
using UnityEngine.UI;

namespace Personal.UI
{
	public class CursorHandler : MonoBehaviour
	{
		[SerializeField] Image cursorImage = null;

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

