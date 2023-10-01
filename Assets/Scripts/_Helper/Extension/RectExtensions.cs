using UnityEngine;

namespace Helper
{
	public static class RectExtensions
	{
		/// <summary>
		/// Transforms a rect from the transform local space to world space.
		/// </summary>
		public static Rect Transform(this Rect r, Transform transform)
		{
			return new Rect
			{
				min = transform.TransformPoint(r.min),
				max = transform.TransformPoint(r.max),
			};
		}

		/// <summary>
		/// Transforms a rect from world space to the transform local space
		/// </summary>
		public static Rect InverseTransform(this Rect r, Transform transform)
		{
			return new Rect
			{
				min = transform.InverseTransformPoint(r.min),
				max = transform.InverseTransformPoint(r.max),
			};
		}
	}
}