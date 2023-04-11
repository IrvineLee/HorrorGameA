using UnityEngine;
using System;
using System.Collections.Generic;

using Personal.Save;

namespace Personal.GameState
{
	[Serializable]
	public class PlayerStateModel
	{
		public int CharacterID { get; private set; }

		public Dictionary<int, string> intStrDictionary { get; private set; } = new();

		public PlayerStateModel(PlayerSavedData data)
		{
			CharacterID = data.CharacterID;

			foreach (var test in data.IntStrDictionary)
			{
				intStrDictionary.Add(test.Key, test.Value);
			}
		}
	}
}