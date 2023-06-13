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

		public PlayerStateModel(PlayerSavedData data)
		{
			CharacterID = data.CharacterID;
		}
	}
}