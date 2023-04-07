using UnityEngine;

namespace Helper
{
	public static class TransformExtensions
	{
		// Replace with
		public static Transform DestroyAllChildren(this Transform original)
		{
			for (int i = original.childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(original.GetChild(i).gameObject);
			}
			return original;
		}
	}
}
