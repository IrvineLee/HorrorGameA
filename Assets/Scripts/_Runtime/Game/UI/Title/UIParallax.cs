using UnityEngine;

using Personal.GameState;
using Personal.Manager;

namespace Personal.UI.Option
{
	public class UIParallax : GameInitialize
	{
		[SerializeField] float moveModifier = 1f;

		Vector3 startPos;
		Camera cam;

		protected override void Initialize()
		{
			startPos = transform.position;
			cam = Camera.main;
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
			var viewportPoint = Vector3.Normalize(InputManager.Instance.GetMotion(InputManager.MotionType.LookAt));
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				viewportPoint = cam.ScreenToViewportPoint(Input.mousePosition);
			}

			var point = new Vector3(startPos.x + (viewportPoint.x * moveModifier), startPos.y + (viewportPoint.y * moveModifier), 0);
			transform.position = point;
		}
	}
}