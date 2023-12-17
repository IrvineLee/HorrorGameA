using System;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Personal.UI;

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

			[SerializeField] TextAlignmentOptions alignment = TextAlignmentOptions.MidlineJustified;

			public string Str { get => str; }
			public TextAlignmentOptions Alignment { get => alignment; }

			public Page(string str, TextAlignmentOptions alignment = TextAlignmentOptions.MidlineJustified)
			{
				this.str = str;
				this.alignment = alignment;
			}
		}

		[SerializeField] ReadFileType readFileType = ReadFileType._100000_File01;
		[SerializeField] List<Page> pageList = new();

		public ReadFileType ReadFileType { get => readFileType; }
		public List<Page> PageList { get => pageList; }
	}
}