using UnityEngine;
using System;

using Helper;

namespace Personal.Save
{
	[Serializable]
	public class PlayerSavedData
	{
		[SerializeField] int slotID = 0;

		[SerializeField] int characterID = 0;


		public int SlotID { get => slotID; set => slotID = value; }

		public int CharacterID { get => characterID; set => characterID = value; }
	}
}