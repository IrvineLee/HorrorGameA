using System;
using UnityEngine;

namespace Personal.GameState
{
	[Serializable]
	public class TargetInfo
	{
		public enum TargetType
		{
			SpawnAt = 0,
			MoveTo,
			LeaveMoveTo,
		}

		[SerializeField] Transform spawnAt = null;
		[SerializeField] Transform moveTo = null;
		[SerializeField] Transform leaveMoveTo = null;

		public Transform SpawnAt { get => spawnAt; }
		public Transform MoveTo { get => moveTo; }
		public Transform LeaveMoveTo { get => leaveMoveTo; }
	}
}