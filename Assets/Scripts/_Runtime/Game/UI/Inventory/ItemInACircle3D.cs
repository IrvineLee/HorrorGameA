using System;
using UnityEngine;

using Helper;
using Personal.InputProcessing;
using Personal.GameState;
using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.UI
{
	public class ItemInACircle3D : GameInitialize, IAngleDirection
	{
		[SerializeField] float radius = 5f;
		[SerializeField] float rotateDuration = 0.25f;

		UIInputController uIInputController;
		float yAngleToRotate;

		CoroutineRun rotateAroundCR = new CoroutineRun();

		protected override async UniTask Awake()
		{
			await base.Awake();

			uIInputController = InputManager.Instance.UIInputController;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			HandleInput();
		}

		protected override async UniTask OnEnable()
		{
			await base.OnEnable();

			SetIntoACircle();
		}

		void HandleInput()
		{
			if (uIInputController.Move == Vector2.zero) return;
			if (!rotateAroundCR.IsDone) return;

			float angle = yAngleToRotate;
			if (uIInputController.Move.x < 0)
			{
				angle = -yAngleToRotate;
			}

			Vector3 angleRotation = new Vector3(0, angle, 0);
			rotateAroundCR = CoroutineHelper.RotateWithinSeconds(transform, angleRotation, rotateDuration, default, false);
		}

		void SetIntoACircle()
		{
			// Put all the children into a circle.
			int childCount = transform.childCount;
			var directionList = IAngleDirection.GetDirectionListFor360Degrees(childCount, 0);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				Vector2 direction = directionList[i] * radius;

				child.position = transform.position;
				child.localPosition = child.localPosition.With(x: direction.x, z: direction.y);
			}

			// Since the first item spawned would be straight ahead, rotate it 180 degrees so it face the front.
			transform.localRotation = Quaternion.identity;
			transform.localEulerAngles = Vector3.zero.With(y: 180);

			Quaternion quaternion = Quaternion.Euler(transform.localEulerAngles);
			transform.localRotation = quaternion;

			yAngleToRotate = 360 / childCount;
		}
	}
}