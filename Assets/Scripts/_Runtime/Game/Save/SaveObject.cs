using System;
using System.Collections.Generic;
using UnityEngine;

using Helper;
using static Personal.InteractiveObject.InteractableInsertItem;

namespace Personal.Save
{
	[Serializable]
	public class SaveObject : GenericSave
	{
		[SerializeField] PlayerSavedData playerSavedData = new();
		[SerializeField] SerializableDictionary<string, bool> pickupableDictionary = new();
		[SerializeField] SerializableDictionary<string, Completed> insertItemDictionary = new();

		public PlayerSavedData PlayerSavedData { get => playerSavedData; }
		public SerializableDictionary<string, bool> PickupableDictionary { get => pickupableDictionary; set => pickupableDictionary = value; }
		public SerializableDictionary<string, Completed> InsertItemDictionary { get => insertItemDictionary; set => insertItemDictionary = value; }
	}
}
