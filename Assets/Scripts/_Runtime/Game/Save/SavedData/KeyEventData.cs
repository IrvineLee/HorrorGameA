using System;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Save
{
	[Serializable]
	public class KeyEventData
	{
		[SerializeField] List<KeyEventType> keyEventList = new();

		public List<KeyEventType> KeyEventList { get => keyEventList; set => keyEventList = value; }
	}
}