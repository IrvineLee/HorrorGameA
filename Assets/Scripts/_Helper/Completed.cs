using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	[Serializable]
	public class Completed
	{
		[SerializeField] List<bool> isCompletedList = new();

		public List<bool> IsCompletedList { get => isCompletedList; }

		public Completed(List<bool> list)
		{
			isCompletedList = list;
		}
	}
}