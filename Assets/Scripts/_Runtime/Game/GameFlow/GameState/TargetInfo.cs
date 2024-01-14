using System;
using UnityEngine;

using Personal.Manager;

namespace Personal.GameState
{
	/// <summary>
	/// This is used for spawning and moving to places.
	/// Typically for NPCs.
	/// </summary>
	[Serializable]
	public class TargetInfo
	{
		public enum TargetType
		{
			SpawnAt = 0,
			MoveTo,
			Leave,
			Player,
		}

		[SerializeField] Transform spawnTarget = null;
		[SerializeField] Transform moveToTarget = null;
		[SerializeField] Transform leaveTarget = null;

		[SerializeField] Transform placeToPutItem = null;

		public Transform SpawnTarget { get => spawnTarget; }
		public Transform MoveToTarget { get => moveToTarget; }
		public Transform LeaveTarget { get => leaveTarget; }
		public Transform Player { get => StageManager.Instance.PlayerController.transform; }

		public Transform PlaceToPutItem { get => placeToPutItem; }
	}
}