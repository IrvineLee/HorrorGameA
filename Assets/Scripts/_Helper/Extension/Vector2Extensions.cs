using UnityEngine;

namespace Helper
{
	public static class Vector2Extensions
	{
		/// <summary>
		/// Replace with
		/// </summary>
		public static Vector2 With(this Vector2 original, float? x = null, float? y = null)
		{
			return new Vector2(x ?? original.x, y ?? original.y);
		}

		/// <summary>
		/// Flatten it with y = 0
		/// </summary>
		public static Vector2 Flat(this Vector2 original)
		{
			return new Vector2(original.x, 0);
		}

		///// <summary>
		///// DirectionTo
		///// </summary>
		//public static Vector2 DirectionTo(this Vector3 source, Vector3 destination)
		//{
		//	return Vector2.Normalize(destination - source);
		//}

		/// <summary>
		/// SpanTo
		/// </summary>
		public static Vector2 SpanTo(this Vector3 source, Vector3 destination)
		{
			return destination - source;
		}

		/// <summary>
		/// Swap
		/// </summary>
		public static Vector2 Swap(this Vector2 original)
		{
			return new Vector2(original.y, original.x);
		}

		/// <summary>
		/// Similar to RotateAround, but does not apply the value. Instead returns back the next position.
		/// </summary>
		public static Vector2 RotateAroundPoint(this Transform source, Vector2 point, Vector2 axis, float value)
		{
			Vector2 finalPos = (Vector2)source.position - point;

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
