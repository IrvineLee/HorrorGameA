using UnityEngine;

using Helper;

namespace HyperCasualGame.Stage
{
	/// <summary>
	/// This moves the object from the current spot to the 'moveRange' and back to the current spot 
	/// and to the opposite side of 'moveRange'. Repeat.
	/// It starts at the center, move to one side, back to center, to the opposite side.
	/// </summary>
	public class MoveLocalRepeat : MonoBehaviour
	{
		[SerializeField] MovementType movementType = MovementType.Transform;
		[SerializeField] Vector3 moveRange = Vector3.zero;

		[Tooltip("ディフォルト所から１方向に行く時間。ディフォルト所に戻る時間。")]
		[SerializeField] float duration = 0.5f;

		[Tooltip("スタート遅延。")]
		[SerializeField] float startDelay = 0;

		[Tooltip("リピート？")]
		[SerializeField] bool isRepeat = true;

		public Vector3 MoveRange { get => moveRange; }
		public float Duration { get => duration; }
		public float StartDelay { get => startDelay; }

		Vector3 startPosition, oppositePosition, nextPosition;

		CoroutineRun cr, disableCR;

		Rigidbody rgBody;
		Rigidbody2D rgBody2D;

		void Awake()
		{
			if (movementType == MovementType.Rigidbody)
				rgBody = GetComponentInChildren<Rigidbody>();
			else if (movementType == MovementType.Rigidbody2D)
				rgBody2D = GetComponentInChildren<Rigidbody2D>();
		}

		void OnEnable()
		{
			startPosition = transform.localPosition;
		}

		public void Initalize(Vector3 moveRange, float duration, float startDelay)
		{
			this.moveRange = moveRange;
			this.duration = duration;
			this.startDelay = startDelay;
		}

		public void EnableMovement()
		{
			disableCR?.StopCoroutine();
			StartMoving();
		}

		public void MoveToStartPositionAndDisableGO()
		{
			MoveToTheOtherEnd(nextPosition, startPosition, 0.5f);
			disableCR = CoroutineHelper.WaitFor(duration, () => gameObject.SetActive(false));
		}

		void StartMoving()
		{
			cr = CoroutineHelper.WaitFor(startDelay, () =>
			{
				if (this == null)
					return;

				oppositePosition = startPosition - moveRange;
				nextPosition = startPosition + moveRange;

				if (movementType == MovementType.Transform)
				{
					cr = CoroutineHelper.LerpTo(transform, nextPosition, duration, () =>
					{
						if (!isRepeat) return;
						MoveToTheOtherEnd(nextPosition, oppositePosition);
					});
				}
				else if (movementType == MovementType.Rigidbody2D)
				{
					float distance = Vector3.Distance(transform.localPosition, nextPosition);
					float speed = distance / duration;
					cr = CoroutineHelper.MoveTo(rgBody2D, nextPosition, speed, () =>
					{
						if (!isRepeat) return;
						MoveToTheOtherEnd(nextPosition, oppositePosition);
					});
				}
			});
		}

		// The duration in this function is multiplied by 2 because it's going back to the center and to the other end.
		void MoveToTheOtherEnd(Vector3 currentPosition, Vector3 nextPosition, float durationMultiplier = 1)
		{
			if (movementType == MovementType.Transform)
			{
				cr = CoroutineHelper.LerpTo(transform, nextPosition, duration * (2f * durationMultiplier), () =>
				{
					if (!isRepeat) return;
					MoveToTheOtherEnd(nextPosition, currentPosition);
				});
			}
			else
			{
				float distance = Vector3.Distance(currentPosition, nextPosition);
				float speed = distance / duration;

				if (movementType == MovementType.Rigidbody)
					cr = CoroutineHelper.MoveTo(rgBody, nextPosition, speed / (2f * durationMultiplier), () =>
					{
						if (!isRepeat) return;
						MoveToTheOtherEnd(nextPosition, currentPosition);
					});
				else if (movementType == MovementType.Rigidbody2D)
					cr = CoroutineHelper.MoveTo(rgBody2D, nextPosition, speed / (2f * durationMultiplier), () =>
					{
						if (!isRepeat) return;
						MoveToTheOtherEnd(nextPosition, currentPosition);
					});
			}
		}

		void OnDisable()
		{
			cr?.StopCoroutine();
			disableCR?.StopCoroutine();
		}
	}
}