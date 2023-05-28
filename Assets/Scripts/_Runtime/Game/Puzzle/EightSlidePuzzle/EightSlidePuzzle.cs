using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Helper;
using Personal.Interface;
using Personal.Manager;

namespace Puzzle.EightSlide
{
	public class EightSlidePuzzle : PuzzleController, IPuzzle, IProcess
	{
		[Serializable]
		class Tile
		{
			[TableColumnWidth(200, false)]
			[ChildGameObjectsOnly(IncludeSelf = false)]
			[SerializeField] Transform tileTrans;

			[SerializeField] int startIndex;
			[SerializeField] int endIndex;

			[ReadOnly] [SerializeField] int currentIndex;

			public Transform TileTrans { get => tileTrans; }
			public int StartIndex { get => startIndex; }
			public int EndIndex { get => endIndex; }
			public int CurrentIndex { get => currentIndex; }

			public void SetStartIndex(int value) { startIndex = value; Initialize(); }

			public void SetCurrentIndex(int value) { currentIndex = value; }

			public void Initialize() { SetCurrentIndex(startIndex); }

			public BasicDirection? GetBasicDirection(int emptyIndex)
			{
				if (currentIndex - 1 == emptyIndex && currentIndex % 3 != 0) return BasicDirection.Left;
				else if (currentIndex + 1 == emptyIndex && (currentIndex + 1) % 3 != 0) return BasicDirection.Right;
				else if (currentIndex - 3 == emptyIndex) return BasicDirection.Up;
				else if (currentIndex + 3 == emptyIndex) return BasicDirection.Down;

				return null;
			}
		}

		[DetailedInfoBox("TileInfo", "Tile index:\n" + "[0,1,2]\n" + "[3,4,5]\n" + "[6,7,8]\n")]
		[TableList] [SerializeField] List<Tile> tileList = new();

		[SerializeField] float slideDuration = 0.5f;

		int emptyIndex;
		CoroutineRun slideCR = new CoroutineRun();

		protected override async UniTask Awake()
		{
			await base.Awake();

			var tempList = new List<int>(new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

			// Set the current and empty index.
			foreach (var tile in tileList)
			{
				tile.Initialize();

				if (!tempList.Contains(tile.StartIndex))
				{
					Debug.LogError("EightSlidePuzzle has duplicate index");
					return;
				}

				tempList.Remove(tile.StartIndex);
			}

			emptyIndex = tempList[0];
		}

		protected override void OnUpdate()
		{
			if (!InputManager.Instance.IsInteract) return;
			if (!slideCR.IsDone) return;

			// Check puzzle click.
			RaycastHit hit;

			Vector2 mousePosition = Mouse.current.position.ReadValue();
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				((IPuzzle)this).ClickedInteractable(hit.transform);
				((IPuzzle)this).CheckPuzzleAnswer();
			}
		}

		/// <summary>
		/// Handle the clicked tile.
		/// </summary>
		/// <param name="trans"></param>
		void IPuzzle.ClickedInteractable(Transform trans)
		{
			foreach (var tile in tileList)
			{
				if (tile.TileTrans.Equals(trans))
				{
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

			await UniTask.WaitUntil(() => slideCR.IsDone);

			puzzleState = PuzzleState.Completed;
			enabled = false;
			Debug.Log("YOU WIN!");
		}

		/// <summary>
		/// Handle whether the puzzle has started.
		/// </summary>
		/// <param name="isFlag"></param>
		void IProcess.Begin(bool isFlag)
		{
			enabled = isFlag;
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
