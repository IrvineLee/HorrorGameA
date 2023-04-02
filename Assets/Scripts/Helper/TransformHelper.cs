using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public class TransformHelper : MonoBehaviour
	{
		/// <summary>
		/// Update the current position of 'axis' (and 'axis2') while keeping the rest the same.
		/// </summary>
		public static void UpdatePosition(Transform instance, char axis, float newVal, Space space = Space.Self, char axis2 = default, float newVal2 = default)
		{
			Vector3 vec = GetUpdatedPosition(instance, axis, newVal, space, axis2, newVal2);

			if (space == Space.Self) instance.localPosition = vec;
			else if (space == Space.World) instance.position = vec;
		}

		/// <summary>
		/// Returns the updated position of 'axis' (and 'axis2') while keeping the rest the same.
		/// </summary>
		public static Vector3 GetUpdatedPosition(Transform instance, char axis, float newVal, Space space = Space.Self, char axis2 = default, float newVal2 = default)
		{
			Vector3 pos = instance.localPosition;
			if (space == Space.World) pos = instance.position;

			pos = ChangeVectorValue(pos, axis, newVal);
			if (axis2 != default && newVal2 != default) pos = ChangeVectorValue(pos, axis, newVal);

			return pos;
		}

		/// <summary>
		/// Rescale current 'trans' scale to 'endScale' within 'duration'.
		/// </summary>
		public static CoroutineRun RescaleTransform(Transform trans, Vector3 endScale, float duration, Action doLast = default)
		{
			return RescaleTransform(trans, trans.localScale, endScale, duration, doLast);
		}

		/// <summary>
		/// Rescale current 'trans' from 'startScale' to 'endScale' within 'duration'.
		/// </summary>
		public static CoroutineRun RescaleTransform(Transform trans, Vector3 startScale, Vector3 endScale, float duration, Action doLast = default)
		{
			Action<Vector3> callbackMethod = (result) => { if (trans) trans.localScale = result; };
			return CoroutineHelper.LerpWithinSeconds(startScale, endScale, duration, callbackMethod, doLast);
		}

		/// <summary>
		/// Move from current 'trans' position to 'endPosition' within 'duration'.
		/// </summary>
		public static CoroutineRun MoveFromTo(Transform trans, Vector2 endPosition, float duration = 1f, Action doLast = default)
		{
			return MoveFromTo(trans, trans.localPosition, endPosition, duration, doLast);
		}

		/// <summary>
		/// Move from current 'trans' from 'startPosition' to 'endPosition' within 'duration'.
		/// </summary>
		public static CoroutineRun MoveFromTo(Transform trans, Vector2 startPosition, Vector2 endPosition, float duration = 1f, Action doLast = default)
		{
			Action<Vector2> callbackMethod = (result) => { if (trans) trans.localPosition = result; };
			return CoroutineHelper.LerpWithinSeconds(startPosition, endPosition, duration, callbackMethod, doLast);
		}

		/// <summary>
		/// Unparent all children of 'trans', move parent to 'toPos', reparent the children back.
		/// </summary>
		public static void Unparent_ChangePos_Reparent(Transform trans, Vector3 toPos)
		{
			List<Transform> childList = new List<Transform>();
			for (int i = trans.childCount - 1; i >= 0; i--)
			{
				Transform child = trans.GetChild(i);
				child.SetParent(null);
				childList.Add(child);
			}

			trans.position = toPos;
			for (int i = 0; i < childList.Count; i++)
			{
				childList[i].SetParent(trans);
			}
		}


		// --------------------------------------------------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------ ALL FUNCTIONS BELOW ARE PRIVATE -----------------------------------------------------------------
		// --------------------------------------------------------------------------------------------------------------------------------------------------------

		static Vector3 ChangeVectorValue(Vector3 vec, char axis, float newVal)
		{
			if (Equals(axis, 'x')) vec.x = newVal;
			else if (Equals(axis, 'y')) vec.y = newVal;
			else if (Equals(axis, 'z')) vec.z = newVal;

			return vec;
		}
	}
}