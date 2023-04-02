
using UnityEngine;

namespace Puzzle
{
	public interface IPuzzle
	{
		void ClickedInteractable(Transform trans);
		void CheckPuzzleAnswer();
	}
}
