using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using Helper;

namespace Personal.Puzzle.EightSlide
{
	public class TilePuzzleController : PuzzleController
	{
		[Serializable]
		protected class Tile
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
			public SpriteRenderer SpriteRenderer
			{
				get
				{
					return tileTrans.GetComponentInChildren<SpriteRenderer>();
				}
			}

			SpriteRenderer spriteRenderer;

			public void SetStartIndex(int value)
			{
				startIndex = value;
				currentIndex = value;
			}

			public void SetCurrentIndex(int value) { currentIndex = value; }
			public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
			{
				this.spriteRenderer.sprite = spriteRenderer.sprite;
			}

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
		[TableList] [SerializeField] protected List<Tile> tileList = new();
	}
}