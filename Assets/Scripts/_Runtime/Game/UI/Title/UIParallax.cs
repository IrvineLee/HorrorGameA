using UnityEngine;

using Personal.GameState;
using Personal.Manager;
using Personal.Setting.Game;
using Helper;

namespace Personal.UI.Option
{
	public class UIParallax : GameInitialize
	{
		[SerializeField] Vector2 moveModifier = Vector2.one;

		Vector3 startPos;
		Camera cam;

		bool isReturnToOrigin;
		Vector3 velocity;

		CoroutineRun parallaxCR = new CoroutineRun();
		GameData gameData;

		void Start()
		{
			startPos = transform.position;
			cam = Camera.main;

			gameData = GameStateBehaviour.Instance.SaveProfile.OptionSavedData.GameData;
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
			HandleParallax();
		}

		void HandleParallax()
		{
			Vector3 viewportPoint;
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				// Mouse controls.
				viewportPoint = cam.ScreenToViewportPoint(Input.mousePosition);
				viewportPoint.x = viewportPoint.x.Convert01ToNeg1Pos1();
				viewportPoint.y = viewportPoint.y.Convert01ToNeg1Pos1();

				if (gameData.IsInvertLookHorizontal) viewportPoint.x = -viewportPoint.x;
				if (gameData.IsInvertLookVertical) viewportPoint.y = -viewportPoint.y;
			}
			else
			{
				// Gamepad controls.
				viewportPoint = Vector3.Normalize(InputManager.Instance.GetMotion(InputManager.MotionType.LookAt, true));
				viewportPoint.y = -viewportPoint.y;

				if (isReturnToOrigin && viewportPoint == Vector3.zero && parallaxCR.IsDone)
				{
					parallaxCR = CoroutineHelper.LerpTo(transform, startPos, 0.5f, space: Space.World);
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