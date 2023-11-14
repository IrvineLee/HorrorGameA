using System;
using UnityEngine;

using Helper;
using static Personal.Puzzle.PuzzleController;

namespace Personal.Save
{
	[Serializable]
	public class SaveObject : GenericSave
	{
		[SerializeField] SlotSavedData slotSavedData = new SlotSavedData();
		[SerializeField] PlayerSavedData playerSavedData = new();

		// These are for scene objects.
		[SerializeField] SerializableDictionary<string, bool> pickupableDictionary = new();
		[SerializeField] SerializableDictionary<string, Completed> insertItemDictionary = new();
		[SerializeField] SerializableDictionary<string, PuzzleState> puzzleDictionary = new();

		public PlayerSavedData PlayerSavedData { get => playerSavedData; }
		public SlotSavedData SlotSavedData { get => slotSavedData; set => slotSavedData = value; }

		public SerializableDictionary<string, bool> PickupableDictionary { get => pickupableDictionary; set => pickupableDictionary = value; }
		public SerializableDictionary<string, Completed> InsertItemDictionary { get => insertItemDictionary; set => insertItemDictionary = value; }
		public SerializableDictionary<string, PuzzleState> PuzzleDictionary { get => puzzleDictionary; set => puzzleDictionary = value; }
	}
}
