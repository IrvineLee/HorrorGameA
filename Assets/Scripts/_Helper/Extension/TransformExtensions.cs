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

		/// <summary>
		/// Set layers to all children.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="layer"></param>
		public static void SetLayerAllChildren(this Transform original, int layer)
		{
			var children = original.GetComponentsInChildren<Transform>(includeInactive: true);
			foreach (var child in children)
			{
				child.gameObject.layer = layer;
			}
		}
	}
}
