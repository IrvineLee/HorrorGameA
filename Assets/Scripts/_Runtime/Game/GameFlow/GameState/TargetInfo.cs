using System;
using System.Collections.Generic;
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
			Leave,
		}

		[SerializeField] List<Transform> spawnAtList = null;
		[SerializeField] List<Transform> moveToList = null;

		[SerializeField] Transform placeToPutItem = null;

		public Transform SpawnAtFirst { get => spawnAtList[0]; }
		public Transform MoveToFirst { get => moveToList[0]; }
		public Transform SpawnAtLast { get => spawnAtList[spawnAtList.Count - 1]; }
		public Transform MoveToLast { get => moveToList[moveToList.Count - 1]; }

		public List<Transform> SpawnAtList { get => spawnAtList; }
		public List<Transform> MoveToList { get => moveToList; }

		public Transform PlaceToPutItem { get => placeToPutItem; }
	}
}