using System;
using UnityEngine;

using Helper;
using Personal.InteractiveObject;
using static Personal.Puzzle.PuzzleController;

namespace Personal.Save
{
	[Serializable]
	public class SceneObjectSavedData
	{
		// These are for scene objects.
		[SerializeField] SerializableDictionary<string, InteractableState> pickupableDictionary = new();
		[SerializeField] SerializableDictionary<string, Completed> insertItemDictionary = new();
		[SerializeField] SerializableDictionary<string, PuzzleState> puzzleDictionary = new();

		public SerializableDictionary<string, InteractableState> PickupableDictionary { get => pickupableDictionary; set => pickupableDictionary = value; }
		public SerializableDictionary<string, Completed> InsertItemDictionary { get => insertItemDictionary; set => insertItemDictionary = value; }
		public SerializableDictionary<string, PuzzleState> PuzzleDictionary { get => puzzleDictionary; set => puzzleDictionary = value; }
	}
}