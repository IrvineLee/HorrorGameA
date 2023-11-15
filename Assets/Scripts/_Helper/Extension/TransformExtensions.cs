using UnityEngine;

namespace Helper
{
	public static class TransformExtensions
	{
		// Destroy all children immediately.
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

		/// <summary>
		/// Checks whether this transform is the first active sibling.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static bool IsFirstActiveSibling(this Transform original)
		{
			foreach (Transform child in original.parent)
			{
				if (!child.gameObject.activeSelf) continue;
				return child == original;
			}
			return false;
		}
	}
}
