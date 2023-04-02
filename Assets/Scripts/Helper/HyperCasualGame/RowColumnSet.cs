using System;
using UnityEngine;

namespace HyperCasualGame.Helper
{
	[Serializable]
	public class RowColumnSet : MonoBehaviour
	{
		[SerializeField] int row;
		[SerializeField] int column;

		public int Row { get => row; }
		public int Column { get => column; }
		public Vector2 Value { get => new Vector2(row, column); }

		public RowColumnSet(int row, int column)
		{
			SetValue(row, column);
		}

		public void SetValue(int row, int column)
		{
			this.row = row;
			this.column = column;
		}
	}
}