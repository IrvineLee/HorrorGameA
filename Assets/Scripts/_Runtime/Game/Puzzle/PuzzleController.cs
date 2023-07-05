using UnityEngine;

using Personal.GameState;

namespace Puzzle
{
	public class PuzzleController : MonoBehaviour
	{
		public enum PuzzleState
		{
			None = 0,
			Completed,
			Failed,
		}
		protected PuzzleState puzzleState = PuzzleState.None;
	}
}
