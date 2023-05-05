using System;
using UnityEngine;

namespace Personal.Character.Player
{
	[Serializable]
	public class ParentMoveFollowChild
	{
		[SerializeField] Transform parentTarget = null;
		[SerializeField] Transform childTarget = null;
		[SerializeField] bool isFollowPosition = true;
		[SerializeField] bool isFollowRotation = true;
		[SerializeField] Vector3 positionOffset = Vector3.zero;

		public Transform ParentTarget { get => parentTarget; }
		public Transform ChildTarget { get => childTarget; }
		public bool IsFollowPosition { get => isFollowPosition; }
		public bool IsFollowRotation { get => isFollowRotation; }
		public Vector3 PositionOffset { get => positionOffset; }
	}
}