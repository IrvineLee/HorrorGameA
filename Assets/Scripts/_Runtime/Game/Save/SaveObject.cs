using System;
using UnityEngine;

namespace Personal.Save
{
	[Serializable]
	public class SaveObject : GenericSave
	{
		[SerializeField] SlotSavedData slotSavedData = new SlotSavedData();
		[SerializeField] PlayerSavedData playerSavedData = new();
		[SerializeField] SceneObjectSavedData sceneObjectSavedData = new();

		public PlayerSavedData PlayerSavedData { get => playerSavedData; }
		public SlotSavedData SlotSavedData { get => slotSavedData; set => slotSavedData = value; }
		public SceneObjectSavedData SceneObjectSavedData { get => sceneObjectSavedData; set => sceneObjectSavedData = value; }
	}
}
