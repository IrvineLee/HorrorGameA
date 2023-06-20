#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Helper
{
	public class CSVDeleteLastLine
	{
		[MenuItem("Tools/CSV Delete Last Line")]
		public static void DeleteLastLineIfEmpty()
		{
			string assetFolder = Application.dataPath;
			if (!Directory.Exists(assetFolder)) return;

			DirectoryInfo directoryInfo = new DirectoryInfo(assetFolder);
			var fileInfo = directoryInfo.GetFiles("*.csv*", SearchOption.AllDirectories);

			foreach (FileInfo file in fileInfo)
			{
				string fileData = File.ReadAllText(file.FullName);
				List<string> dataList = fileData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

				int lastIndex = dataList.Count - 1;

				// Somehow just removing all the white spaces on the dataList itself does not seem to work.
				// Have to remove and add it again to remove the space.
				string s = dataList[lastIndex];
				dataList.RemoveAt(lastIndex);

				s = s.RemoveAllWhiteSpaces();
				dataList.Add(s);

				File.WriteAllText(file.FullName, string.Join('\n', dataList));
			}
			AssetDatabase.Refresh();
		}
	}
}
#endif