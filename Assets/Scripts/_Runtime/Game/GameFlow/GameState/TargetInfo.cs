using Personal.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

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

		[SerializeField] List<Transform> spawnAtList = null;
		[SerializeField] List<Transform> moveToList = null;
		[SerializeField] List<Transform> leaveList = null;

		[SerializeField] Transform placeToPutItem = null;

		public Transform SpawnAtFirst { get => spawnAtList[0]; }
		public Transform MoveToFirst { get => moveToList[0]; }
		public Transform LeaveAtFirst { get => leaveList[0]; }
		public Transform Player { get => StageManager.Instance.PlayerController.transform; }

		public List<Transform> SpawnAtList { get => spawnAtList; }
		public List<Transform> MoveToList { get => moveToList; }

		public Transform PlaceToPutItem { get => placeToPutItem; }
	}
}