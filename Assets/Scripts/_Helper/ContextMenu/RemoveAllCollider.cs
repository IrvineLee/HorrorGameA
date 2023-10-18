using UnityEngine;

namespace Helper
{
	public class RemoveAllCollider : MonoBehaviour
	{
		[ContextMenu("Delete3DCollider")]
		void Delete3DColliders()
		{
			var colliderList = gameObject.GetComponentsRecursive<Collider>();
			foreach (var collider in colliderList)
			{
				DestroyImmediate(collider);
			}
			Debug.Log("Deleted colliders");
		}
	}
}