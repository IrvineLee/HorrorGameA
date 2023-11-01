using System;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Personal.Data
{
	/// <summary>
	/// This contains the data when you read from pieces of paper/book in game.
	/// </summary>
	[CreateAssetMenu(fileName = "FileData", menuName = "ScriptableObjects/FileData", order = 0)]
	public class FileData : ScriptableObject
	{
		[Serializable]
		public class Page
		{
			[TextArea(3, 10)]
			[SerializeField] string str = "";

			[SerializeField] TextAlignmentOptions alignment = TextAlignmentOptions.Midline;

			public string Str { get => str; }
			public TextAlignmentOptions Alignment { get => alignment; }
		}


		[SerializeField] List<Page> pageList = new();

		public List<Page> PageList { get => pageList; }
	}
}