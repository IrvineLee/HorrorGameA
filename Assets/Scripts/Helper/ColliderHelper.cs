using UnityEngine;

namespace Helper
{
	public class ColliderHelper : MonoBehaviour
	{
		/// <summary>
		/// Copy value from 'source' to 'pasteOnto'. Box, capsule, sphere.
		/// </summary>
		public static void CopyValueOnto(Collider source, Collider pasteOnto)
		{
			pasteOnto.enabled = source.enabled;
			pasteOnto.isTrigger = source.isTrigger;

			HandleBoxColliderCopy(source, pasteOnto);
			HandleCapsuleColliderCopy(source, pasteOnto);
			HandleSphereColliderCopy(source, pasteOnto);
		}

		/// <summary>
		/// Copy value from 'source' to 'pasteOnto'.
		/// </summary>
		public static void CopyValueOnto(Rigidbody source, Rigidbody pasteOnto)
		{
			pasteOnto.mass = source.mass;
			pasteOnto.drag = source.drag;
			pasteOnto.angularDrag = source.angularDrag;
			pasteOnto.useGravity = source.useGravity;
			pasteOnto.isKinematic = source.isKinematic;
			pasteOnto.interpolation = source.interpolation;
			pasteOnto.collisionDetectionMode = source.collisionDetectionMode;
			pasteOnto.constraints = source.constraints;
		}

		static void HandleBoxColliderCopy(Collider source, Collider pasteOnto)
		{
			BoxCollider sourceBoxCollider = source.GetComponent<BoxCollider>();
			if (sourceBoxCollider)
			{
				BoxCollider pasteOntoBoxCollider = pasteOnto.GetComponent<BoxCollider>();
				pasteOntoBoxCollider.center = sourceBoxCollider.center;
				pasteOntoBoxCollider.size = sourceBoxCollider.size;
			}
		}

		static void HandleCapsuleColliderCopy(Collider source, Collider pasteOnto)
		{
			CapsuleCollider sourceCapsuleCollider = source.GetComponent<CapsuleCollider>();
			if (sourceCapsuleCollider)
			{
				CapsuleCollider pasteOntoCapsuleCollider = pasteOnto.GetComponent<CapsuleCollider>();
				pasteOntoCapsuleCollider.center = sourceCapsuleCollider.center;
				pasteOntoCapsuleCollider.radius = sourceCapsuleCollider.radius;
				pasteOntoCapsuleCollider.height = sourceCapsuleCollider.height;
				pasteOntoCapsuleCollider.direction = sourceCapsuleCollider.direction;
			}
		}

		static void HandleSphereColliderCopy(Collider source, Collider pasteOnto)
		{
			SphereCollider sourceSphereCollider = source.GetComponent<SphereCollider>();
			if (sourceSphereCollider)
			{
				SphereCollider pasteOntoSphereCollider = pasteOnto.GetComponent<SphereCollider>();
				pasteOntoSphereCollider.center = sourceSphereCollider.center;
				pasteOntoSphereCollider.radius = sourceSphereCollider.radius;
			}
		}
	}
}