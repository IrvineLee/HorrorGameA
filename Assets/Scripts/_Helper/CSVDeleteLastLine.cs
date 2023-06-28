#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


namespace Helper
{
	public class CSVDeleteLastLine
	{
		[MenuItem("Tools/CSV Delete Empty Lines")]
		public static void DeleteLastLineIfEmpty()
		{
			string assetFolder = Application.dataPath + "/Data/UILocalization";
			if (!Directory.Exists(assetFolder)) return;

			DirectoryInfo directoryInfo = new DirectoryInfo(assetFolder);
			var fileInfo = directoryInfo.GetFiles("*.csv", SearchOption.AllDirectories);

			foreach (FileInfo file in fileInfo)
			{
				string fileData = File.ReadAllText(file.FullName);
				List<string> dataList = fileData.Split(new char[] { '\n' }).ToList();

				// Check whether to remove the line.
				for (int i = dataList.Count - 1; i >= 0; i--)
				{
					string s;
					s = dataList[i].RemoveAllWhiteSpaces();

					if (string.IsNullOrEmpty(s))
					{
						dataList.RemoveAt(i);
					}
				}

				// Make sure there are no additional lines added to the file.
				string dataStr = dataList[0].Trim();
				for (int i = 1; i < dataList.Count; i++)
				{
					dataStr += "\n" + dataList[i].Trim();
				}

				File.WriteAllText(file.FullName, dataStr, Encoding.UTF8);
			}
			AssetDatabase.Refresh();
		}
	}
}
#endif