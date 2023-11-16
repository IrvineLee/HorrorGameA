
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Puzzle
{
	public interface IPuzzle
	{
		void ClickedInteractable(Transform trans);
		void CancelSelected() { }
		void ResetToDefault() { }       // This is used to reset the puzzle back to it's initial state.
		void CheckPuzzleAnswer();
		void AutoComplete();            // This is typically used when loading the game/debug time.
		List<Transform> GetInteractableObjectList();
	}
}
