using UnityEngine;

namespace Personal.Character.Player
{
	public class ParentMoveFollowChild : MonoBehaviour
	{
		[SerializeField] Transform parentTarget = null;
		[SerializeField] Transform childTarget = null;
		[SerializeField] bool isFollowPosition = true;
		[SerializeField] bool isFollowRotation = true;
		[SerializeField] Vector3 positionOffset = Vector3.zero;

		void Update()
		{
			if (!childTarget) return;

			if (isFollowPosition)
			{
				parentTarget.position = childTarget.position + positionOffset;
				childTarget.localPosition = Vector3.zero;
			}

			if (isFollowRotation)
			{
				parentTarget.rotation = childTarget.rotation;
				childTarget.localRotation = Quaternion.identity;
			}
		}
	}
}