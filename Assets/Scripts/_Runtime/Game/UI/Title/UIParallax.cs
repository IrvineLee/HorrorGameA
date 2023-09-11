using UnityEngine;

using Personal.GameState;
using Personal.Manager;
using Helper;

namespace Personal.UI.Option
{
	public class UIParallax : GameInitialize
	{
		[SerializeField] Vector2 moveModifier = Vector2.one;

		Vector3 startPos;
		Camera cam;

		bool isReturnToOrigin;
		Vector3 velocity = Vector3.zero;

		CoroutineRun parallaxCR = new CoroutineRun();

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
			var viewportPoint = Vector3.zero;
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				viewportPoint = cam.ScreenToViewportPoint(Input.mousePosition);
				viewportPoint.x = viewportPoint.x.Convert01ToNeg1Pos1();
				viewportPoint.y = viewportPoint.y.Convert01ToNeg1Pos1();
			}
			else
			{
				viewportPoint = Vector3.Normalize(InputManager.Instance.GetMotion(InputManager.MotionType.LookAt));

				if (isReturnToOrigin && viewportPoint == Vector3.zero && parallaxCR.IsDone)
				{
					parallaxCR = CoroutineHelper.LerpTo(transform, Vector3.zero, 0.5f, () => isReturnToOrigin = false);
					return;
				}
				else if (viewportPoint != Vector3.zero)
				{
					if (!parallaxCR.IsDone) parallaxCR.StopCoroutine();
					if (!isReturnToOrigin) isReturnToOrigin = true;
				}
			}

			var point = new Vector3(startPos.x + (viewportPoint.x * moveModifier.x), startPos.y + (viewportPoint.y * moveModifier.y), 0);
			transform.position = Vector3.SmoothDamp(transform.position, point, ref velocity, 0.15f); ;
		}
	}
}