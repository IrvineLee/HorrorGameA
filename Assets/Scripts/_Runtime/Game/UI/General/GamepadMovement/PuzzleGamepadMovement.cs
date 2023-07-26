using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Personal.Manager;
using Personal.GameState;
using Helper;
using Personal.Constant;
using Personal.UI;

namespace Personal.Puzzle
{
	public class PuzzleGamepadMovement : UIGamepadMovement
	{
		[SerializeField] int startIndex = 0;

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

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				eventTriggerList[0].OnPointerEnter(null);
			}
		}

		protected override void HandleMovement(Vector2 move)
		{
			int nextIndex = -1;
			float shortestSqrMagnitude = float.MaxValue;
			Vector3 currentPosition = eventTriggerList[currentActiveIndex].transform.position;

			Debug.Log(eventTriggerList.Count);
			for (int i = 0; i < eventTriggerList.Count; i++)
			{
				Vector3 triggerPosition = eventTriggerList[i].transform.position;
				if (move.x > 0 && triggerPosition.x > currentPosition.x ||
					move.x < 0 && triggerPosition.x < currentPosition.x ||
					move.y > 0 && triggerPosition.y > currentPosition.y ||
					move.y < 0 && triggerPosition.y < currentPosition.y)
				{
					if (!IsDistanceShorterThan(ref shortestSqrMagnitude, triggerPosition, currentPosition)) continue;
					nextIndex = i;
				}
			}

			if (nextIndex < 0) return;
			eventTriggerList[currentActiveIndex].OnPointerExit(null);
			eventTriggerList[nextIndex].OnPointerEnter(null);
		}

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
