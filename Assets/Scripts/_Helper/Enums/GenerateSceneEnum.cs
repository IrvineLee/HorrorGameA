#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;

namespace Helper
{
	public class GenerateSceneEnum
	{
		[MenuItem("Tools/Generate Scene Enum")]
		public static void Generate()
		{
			string enumName = "SceneType";
			List<string> enumTypeList = new List<string>();
			string filePathAndName = "Assets/Scripts/GenerateCode/" + enumName + ".cs"; //The folder Scripts/Enums/ is expected to exist

			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
				enumTypeList.Add(sceneName);
			}

			using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
			{
				streamWriter.WriteLine("using Helper;\n");

				streamWriter.WriteLine("namespace Personal.Constant");
				streamWriter.WriteLine("{");
				streamWriter.WriteLine("\tpublic enum " + enumName);
				streamWriter.WriteLine("\t{");
				for (int i = 0; i < enumTypeList.Count; i++)
				{
					string name = enumTypeList[i];
					streamWriter.WriteLine("\t\t[StringValue(\"" + name + "\")]");
					streamWriter.WriteLine("\t\t" + name + ",");

					if (i >= enumTypeList.Count - 1) break;
					streamWriter.WriteLine("");
				}
				streamWriter.WriteLine("\t}");
				streamWriter.WriteLine("}");
			}
			AssetDatabase.Refresh();
		}
	}
}
#endif