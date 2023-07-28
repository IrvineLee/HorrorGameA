using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Personal.Manager;
using Helper;
using Personal.UI;

namespace Personal.Puzzle
{
	public class PuzzleGamepadMovement : UIGamepadMovement
	{
		[SerializeField] int startIndex = 0;
		[SerializeField] Axis compareAxis = Axis.XY;

		[Tooltip("This clamps the selection arc for the supposedly next object. " +
			"Ex: A value of 1 means the next object should be directly at the direction of input. " +
			"Pressing down(0, -1) means the x-axis of both object should be exactly the same to register as possible selection. " +
			"Value of 0 is around 180 degrees, 0.5 is around 90 degrees, 1 is around 0 degrees at input direction.")]
		[Range(0, 1)]
		[SerializeField] float selectionArcDotProduct = 0.75f;

		List<EventTrigger> eventTriggerList = new();

		protected override void Initialize()
		{
			base.Initialize();

			List<Transform> interactableObjectList = GetComponentInChildren<IPuzzle>().GetInteractableObjectList();
			eventTriggerList = interactableObjectList.Select((obj) => obj.GetComponentInChildren<EventTrigger>()).ToList();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			currentActiveIndex = startIndex;

			if (InputManager.Instance.IsCurrentDeviceMouse) return;
			eventTriggerList[startIndex].OnPointerEnter(null);
		}

		protected override void HandleMovement(Vector2 move)
		{
			int nextIndex = -1;
			float shortestSqrMagnitude = float.MaxValue;
			Vector3 currentPosition = eventTriggerList[currentActiveIndex].transform.position;

			for (int i = 0; i < eventTriggerList.Count; i++)
			{
				Vector3 triggerPosition = eventTriggerList[i].transform.position;
				if (currentActiveIndex == i) continue;

				if (!IsMovementPossible(move, currentPosition, triggerPosition)) continue;
				if (!IsDistanceShorterThan(ref shortestSqrMagnitude, triggerPosition, currentPosition)) continue;

				nextIndex = i;
			}

			if (nextIndex < 0) return;

			eventTriggerList[currentActiveIndex].OnPointerExit(null);
			eventTriggerList[nextIndex].OnPointerEnter(null);
			currentActiveIndex = nextIndex;
		}

		/// <summary>
		/// Movement are free to go anywhere. Analog input will have a slow start, so just put it to the max value.
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		protected override Vector2 GetHorizontalVericalMovement(Vector2 move)
		{
			if (move.x > 0.1f)
			{
				move.x = move.x > 0 ? 1 : -1;
			}
			if (move.y > 0.1f)
			{
				move.y = move.y > 0 ? 1 : -1;
			}
			return move;
		}

		/// <summary>
		/// Check to see whether movement selection is possible.
		/// </summary>
		/// <param name="move"></param>
		/// <param name="currentPosition"></param>
		/// <param name="triggerPosition"></param>
		/// <returns></returns>
		bool IsMovementPossible(Vector2 move, Vector3 currentPosition, Vector3 triggerPosition)
		{
			// Normalize the direction of from target -> to target.
			Vector3 direction = Vector3.Normalize(triggerPosition - currentPosition);

			// Change the input direction into vector3 and normalize it.
			Vector3 activeFaceDirection = move.normalized;
			if (compareAxis == Axis.XZ) activeFaceDirection = ((Vector3)move).With(y: 0, z: move.y).normalized;

			// Get the dot product and checks whether it's between the selection arc.
			float dotProduct = Vector3.Dot(activeFaceDirection, direction);
			if (dotProduct < selectionArcDotProduct) return false;

			return true;
		}

		/// <summary>
		/// Save the shortest distance(squared magnitude).
		/// </summary>
		/// <param name="shortestSqrMagnitude"></param>
		/// <param name="target1"></param>
		/// <param name="target2"></param>
		/// <returns></returns>
		bool IsDistanceShorterThan(ref float shortestSqrMagnitude, Vector3 target1, Vector3 target2)
		{
			Vector3 distance = target1 - target2;
			if (shortestSqrMagnitude > distance.sqrMagnitude)
			{
				shortestSqrMagnitude = distance.sqrMagnitude;
				return true;
			}
			return false;
		}

		protected override void OnDeviceChanged()
		{
			if (InputManager.Instance.IsCurrentDeviceMouse)
			{
				eventTriggerList[currentActiveIndex].OnPointerExit(null);
				return;
			}
			eventTriggerList[currentActiveIndex].OnPointerEnter(null);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			eventTriggerList[currentActiveIndex].OnPointerExit(null);
		}
	}
}