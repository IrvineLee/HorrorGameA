using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public class FollowTarget : MonoBehaviour
	{
		[SerializeField] Transform target = null;
		[SerializeField] bool isFollowPosition = true;
		[SerializeField] bool isFollowRotation = true;

		public Transform Target { get => target; }

		public void SetTartget(Transform target) { this.target = target; }

		void Update()
		{
			if (!target) return;

			if (isFollowPosition)
				transform.position = target.position;

			if (isFollowRotation)
				transform.rotation = target.rotation;
		}
	}
}
