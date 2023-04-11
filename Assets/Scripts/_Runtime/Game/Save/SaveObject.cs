using System;
using UnityEngine;

namespace Personal.Save
{
	[Serializable]
	public class SaveObject : GenericSave
	{
		[SerializeField] SlotSavedData slotSavedData = new SlotSavedData();
		[SerializeField] PlayerSavedData playerSavedData = new PlayerSavedData();

		public SlotSavedData SlotSavedData { get => slotSavedData; }
		public PlayerSavedData PlayerSavedData { get => playerSavedData; }
	}
}
