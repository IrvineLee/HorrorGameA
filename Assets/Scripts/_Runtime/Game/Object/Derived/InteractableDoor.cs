using System;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.System.Handler;
using Helper;

namespace Personal.InteractiveObject
{
	public class InteractableDoor : InteractableObject
	{
		[SerializeField] Transform doorHingeTrans = null;
		[SerializeField] float duration = 0.5f;
		[SerializeField] float openAngle = 90;
		[SerializeField] bool isOpened = false;

		public event Action OnDoorOpened;
		public event Action OnDoorClosed;

		CoroutineRun runCR = new CoroutineRun();

		protected override UniTask HandleInteraction()
		{
			if (!runCR.IsDone) return UniTask.CompletedTask;

			if (!isOpened)
			{
				// Open the door.
				Physics.IgnoreLayerCollision((int)LayerType._Default, gameObject.layer, true);
				OnDoorOpened?.Invoke();
			}

			// Get the end euler angle.
			Vector3 eulerAngle = doorHingeTrans.localEulerAngles;
			eulerAngle = eulerAngle.With(y: eulerAngle.y + GetDoorMoveAngle());

			// Rotate it to endRotation.
			Quaternion endRotation = Quaternion.Euler(eulerAngle);
			runCR = CoroutineHelper.QuaternionLerpWithinSeconds(doorHingeTrans, doorHingeTrans.rotation, endRotation, duration, () =>
			{
				isOpened = !isOpened;
				if (!isOpened)
				{
					// Close the door.
					Physics.IgnoreLayerCollision((int)LayerType._Default, gameObject.layer, false);
					OnDoorClosed?.Invoke();
				}
			});

			return UniTask.CompletedTask;
		}

		float GetDoorMoveAngle()
		{
			// Get the direction to open the door based on initiator position.
			Vector3 direction = transform.position.GetNormalizedDirectionTo(InitiatorStateMachine.transform.position);

			float dotProduct = Vector3.Dot(transform.forward, direction);
			int wholeNumber = dotProduct > 0 ? 1 : -1;

			float angle = openAngle * wholeNumber;

			if (wholeNumber < 0) return isOpened ? -angle : angle;
			return isOpened ? angle : -angle;
		}
	}
}