using UnityEngine;

using Personal.GameState;

namespace Personal.UI.Option
{
	public class UIParallax : GameInitialize
	{
		[SerializeField] float moveModifier = 1f;

		Vector3 startPos;

		protected override void Initialize()
		{
			startPos = transform.position;
		}

		protected override void OnTitleScene()
		{
			enabled = true;
		}

		protected override void OnMainScene()
		{
			enabled = false;
		}

		void Update()
		{
			var viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			var point = new Vector3(startPos.x + (viewportPoint.x * moveModifier), startPos.y + (viewportPoint.y * moveModifier), 0);
			transform.position = point;
		}
	}
}