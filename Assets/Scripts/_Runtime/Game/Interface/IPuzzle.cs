
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Puzzle
{
	public interface IPuzzle
	{
		void ClickedInteractable(Transform trans);
		void CheckPuzzleAnswer();
		List<Transform> GetInteractableObjectList();
	}
}
