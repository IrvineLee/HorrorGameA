using System;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Interface;
using Helper;

namespace Personal.Puzzle.EightSlide
{
	public class SwapNumbersPuzzle : TilePuzzleController, IPuzzle, IProcess
	{
		Dictionary<Transform, Tile> tileDictionary = new();

		Tile activeSelection;    // This handle the initial selection.

		protected override void Awake()
		{
			base.Awake();
			var tempList = new List<int>(new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

			// Set the current and empty index.
			foreach (var tile in tileList)
			{
				tile.SetStartIndex(tile.StartIndex);

				tileDictionary.Add(tile.TileTrans, tile);
				if (!tempList.Contains(tile.StartIndex))
				{
					Debug.LogError("SwapNumbersPuzzle has duplicate index");
					return;
				}

				tempList.Remove(tile.StartIndex);
			}
		}

		protected override Transform GetActiveSelectionForGamepad()
		{
			foreach (Tile tile in tileDictionary.Values)
			{
				if (tile.StartIndex == puzzleGamepadMovement.CurrentActiveIndex) return tile.TileTrans;
			}
			return null;
		}

		public List<Transform> GetInteractableObjectList()
		{
			return tileList.ConvertAll(tile => tile.TileTrans);
		}

		/// <summary>
		/// Handle the clicked tile.
		/// </summary>
		/// <param name="trans"></param>
		void IPuzzle.ClickedInteractable(Transform trans)
		{
			if (!tileDictionary.TryGetValue(trans, out Tile tile)) return;

			TryMoveTile(tile);
		}

		/// <summary>
		/// Try to move tiles when possible.
		/// </summary>
		/// <param name="tile"></param>
		void TryMoveTile(Tile tile)
		{
			if (activeSelection == null)
			{
				activeSelection = tile;
				activeSelection.SpriteRenderer.color = Color.white * 0.75f;

				return;
			}

			// Reset the previously selected selection.
			activeSelection.SpriteRenderer.color = Color.white;

			// Update tile index.
			int index = activeSelection.CurrentIndex;
			activeSelection.SetCurrentIndex(tile.CurrentIndex);
			tile.SetCurrentIndex(index);

			// Swap the positions.
			Vector3 temp = activeSelection.TileTrans.position;
			activeSelection.TileTrans.position = tile.TileTrans.position;
			tile.TileTrans.position = temp;

			puzzleGamepadMovement.UpdateCurrentSelection(activeSelection.TileTrans.gameObject);

			activeSelection = null;
		}

		/// <summary>
		/// Check puzzle answer.
		/// </summary>
		async void IPuzzle.CheckPuzzleAnswer()
		{
			foreach (var tile in tileList)
			{
				if (tile.CurrentIndex != tile.EndIndex)
					return;
			}

			await UniTask.WaitUntil(() => slideCR.IsDone, cancellationToken: this.GetCancellationTokenOnDestroy());

			puzzleState = PuzzleState.Completed;
			enabled = false;

			GetReward();
		}

		/// <summary>
		/// Cancelled the pick.
		/// </summary>
		/// <param name="trans"></param>
		void IPuzzle.CancelledInteractable(Transform trans)
		{
			if (activeSelection == null) return;

			// Need to wait for the for the IProcess.IsExit to get called first.
			CoroutineHelper.WaitEndOfFrame(() =>
			{
				activeSelection.SpriteRenderer.color = Color.white;
				activeSelection = null;
			});
		}

		/// <summary>
		/// Handle whether the puzzle has started.
		/// </summary>
		/// <param name="isFlag"></param>
		void IProcess.Begin(bool isFlag)
		{
			enabled = isFlag;
			HandleMouseOrGamepadDisplay(isFlag);

			if (puzzleState == PuzzleState.Completed) return;
			puzzleState = PuzzleState.None;
		}

		/// <summary>
		/// Return if the puzzle has been completed.
		/// </summary>
		/// <returns></returns>
		bool IProcess.IsCompleted()
		{
			return puzzleState == PuzzleState.Completed;
		}

		/// <summary>
		/// Return when failed.
		/// </summary>
		/// <returns></returns>
		bool IProcess.IsFailed()
		{
			return puzzleState == PuzzleState.Failed;
		}

		/// <summary>
		/// Exiting puzzle.
		/// </summary>
		/// <returns></returns>
		bool IProcess.IsExit()
		{
			return activeSelection == null;
		}
	}
}
