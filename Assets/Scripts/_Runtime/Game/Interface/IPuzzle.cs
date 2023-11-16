
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Puzzle
{
	public interface IPuzzle
	{
		void ClickedInteractable(Transform trans);
		void CancelledInteractable(Transform trans) { }
		void CheckPuzzleAnswer();
		void AutoComplete();          // This is typically used when loading the game/debug time.
		List<Transform> GetInteractableObjectList();
	}
}
