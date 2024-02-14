using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Helper;
using Personal.InputProcessing;

namespace Personal.Puzzle.EightSlide
{
	public class EightSlidePuzzle : TilePuzzleController, IPuzzle
	{
		[SerializeField] SelectionTransformSet selectionTransformSet = null;
		[SerializeField] float slideDuration = 0.5f;

		int emptyIndex;
		int defaultEmptyIndex;

		Dictionary<Transform, Tile> tileDictionary = new();

		protected override void Initialize()
		{
			base.Initialize();
			if (puzzleState == PuzzleState.Completed) return;

			var tempList = new List<int>(new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

			// Set the current and empty index.
			foreach (var tile in tileList)
			{
				tile.Init();
				tile.SetCurrentIndex(tile.StartIndex);

				selectionTransformSet.SetInitialTarget(tile.StartIndex, tile.TileTrans);
				tileDictionary.Add(tile.TileTrans, tile);

				if (!tempList.Contains(tile.StartIndex))
				{
					Debug.LogError("EightSlidePuzzle has duplicate index");
					return;
				}

				tempList.Remove(tile.StartIndex);
			}

			defaultEmptyIndex = emptyIndex = tempList[0];
			selectionTransformSet.Init();
		}

		public override Transform GetActiveSelectionForGamepad()
		{
			// The reason why you don't wanna return back the Target as selection is because it needs to handle the mouse control as well.
			int index = ((ControlInput)ControlInputBase.ActiveControlInput).CurrentActiveIndex;
			return selectionTransformSet.SelectionTargetList[index].Selection;
		}

		public List<Transform> GetInteractableObjectList()
		{
			return selectionTransformSet.SelectionTargetList.Select(selection => selection.Selection).ToList();
		}

		/// <summary>
		/// Handle the clicked tile.
		/// </summary>
		/// <param name="trans"></param>
		void IPuzzle.ClickedInteractable(Transform trans)
		{
			foreach (var selection in selectionTransformSet.SelectionTargetList)
			{
				if (selection.Selection.Equals(trans))
				{
					if (selection.Target == null) return;
					if (!tileDictionary.TryGetValue(selection.Target, out Tile tile)) return;

					TryMoveTile(tile);
					break;
				}
			}
		}

		/// <summary>
		/// Try to move tiles when possible.
		/// </summary>
		/// <param name="tile"></param>
		void TryMoveTile(Tile tile)
		{
			BasicDirection? basicDirection = tile.GetBasicDirection(emptyIndex);
			if (basicDirection == null) return;

			Vector3 pos = tile.TileTrans.localPosition;

			// Get the next position.
			Vector3 nextPosition = pos + Vector3.up;
			if (basicDirection == BasicDirection.Down) nextPosition = pos + Vector3.down;
			else if (basicDirection == BasicDirection.Left) nextPosition = pos + Vector3.left;
			else if (basicDirection == BasicDirection.Right) nextPosition = pos + Vector3.right;

			slideCR = CoroutineHelper.LerpFromTo(tile.TileTrans, tile.TileTrans.localPosition, nextPosition, slideDuration);

			// Update tile index.
			int index = emptyIndex;
			emptyIndex = tile.CurrentIndex;
			tile.SetCurrentIndex(index);

			selectionTransformSet.SwapTargetToEmpty(tile.TileTrans, emptyIndex);
		}

		/// <summary>
		/// Cancelled the puzzle.
		/// </summary>
		void IPuzzle.CancelSelected()
		{
			puzzleState = PuzzleState.Failed;
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

			EndAndGetReward();
		}

		/// <summary>
		/// Complete the puzzle.
		/// </summary>
		void IPuzzle.AutoComplete()
		{
			Dictionary<int, Transform> indexTransformDictionary = new();
			for (int i = 0; i < selectionTransformSet.SelectionTargetList.Count; i++)
			{
				Transform selection = selectionTransformSet.SelectionTargetList[i].Selection;
				indexTransformDictionary.Add(i, selection);
			}

			foreach (var tile in tileList)
			{
				if (!indexTransformDictionary.TryGetValue(tile.EndIndex, out Transform selection)) continue;

				// Update tile index.
				tile.SetCurrentIndex(tile.EndIndex);
				tile.TileTrans.position = selection.position;
			}
			EndAndGetReward();
		}

		/// <summary>
		/// Reset to default.
		/// </summary>
		void IPuzzle.ResetToDefault()
		{
			selectionTransformSet.Reset();

			foreach (var tile in tileList)
			{
				tile.Reset();
				selectionTransformSet.SetInitialTarget(tile.StartIndex, tile.TileTrans);
			}

			emptyIndex = defaultEmptyIndex;
			selectionTransformSet.Init();
		}

		protected override void OnBegin(bool isFlag)
		{
			base.OnBegin(isFlag);
			selectionTransformSet.gameObject.SetActive(isFlag);
		}

		/// <summary>
		/// Arrange the tiles based on the index.
		/// </summary>
		[Button("Arrange Tiles (Manual)", Icon = SdfIconType.Tools, IconAlignment = IconAlignment.RightEdge, ButtonAlignment = 1f)]
		void ArrangeTiles()
		{
			for (int i = 0; i < tileList.Count; i++)
			{
				Tile tile = tileList[i];
				float x = 1;
				float y = -1;

				if (tile.StartIndex % 3 == 0) x = -1;
				else if ((tile.StartIndex - 1) % 3 == 0) x = 0;

				if (tile.StartIndex < 3) y = 1;
				else if (tile.StartIndex < 6) y = 0;

				tile.TileTrans.localPosition = tile.TileTrans.localPosition.With(x: x, y: y);
			}
		}

		/// <summary>
		/// Randomly set the start indexes and arrange it.
		/// </summary>
		[Button("Random Start Index (Automatic)", Icon = SdfIconType.Dice6Fill, IconAlignment = IconAlignment.RightEdge, ButtonAlignment = 1f)]
		void RandomStartIndex()
		{
			var randomList = CodeHelper.GenerateRandomList(0, 9);
			for (int i = 0; i < tileList.Count; i++)
			{
				tileList[i].SetStartIndex(randomList[i]);
			}

			ArrangeTiles();
		}
	}
}
