using System;
using UnityEngine;

using Helper;

namespace Personal.Save
{
	[Serializable]
	public class SaveObject : GenericSave
	{
		[SerializeField] PlayerSavedData playerSavedData = new();
		[SerializeField] SerializableDictionary<string, bool> pickupableDictionary = new();

		public PlayerSavedData PlayerSavedData { get => playerSavedData; }
		public SerializableDictionary<string, bool> PickupableDictionary { get => pickupableDictionary; set => pickupableDictionary = value; }
	}
}
