
using System;
using UnityEngine;

namespace Personal.Quest
{
	[Serializable]
	public class QuestInfo
	{
		[SerializeField] string test;

		public string Test { get; private set; }

		public QuestInfo(string test)
		{
			Test = test;
		}
	}
}