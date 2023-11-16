using UnityEngine;

namespace Helper
{
	public static class Vector3Extensions
	{
		/// <summary>
		/// Replace with
		/// </summary>
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
		}

		/// <summary>
		/// Get normalized direction.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="targetPosition">The target position</param>
		/// <returns></returns>
		public static Vector3 GetNormalizedDirectionTo(this Vector3 original, Vector3 targetPosition)
		{
			return (targetPosition - original).normalized;
		}

		/// <summary>
		/// Flatten it with y = 0
		/// </summary>
		public static Vector3 Flat(this Vector3 original)
		{
			return new Vector3(original.x, 0, original.z);
		}

		/// <summary>
		/// DirectionTo
		/// </summary>
		public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
		{
			return Vector3.Normalize(destination - source);
		}

		/// <summary>
		/// SpanTo
		/// </summary>
		public static Vector3 SpanTo(this Vector3 source, Vector3 destination)
		{
			return destination - source;
		}

		/// <summary>
		/// Similar to RotateAround, but does not apply the value. Instead returns back the next position.
		/// </summary>
		public static Vector3 RotateAroundPoint(this Transform source, Vector3 point, Vector3 axis, float value)
		{
			Vector3 finalPos = source.position - point;

			Quaternion angle = Quaternion.Euler(axis * value);

			// Center the point around the origin
			finalPos = angle * finalPos;

			// Rotate the point.
			finalPos += point;

			// Move the point back to its original offset. 
			return finalPos;
		}
	}
}
