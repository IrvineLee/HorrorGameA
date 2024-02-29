#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Helper
{
	public class GenerateSceneEnum
	{
		[MenuItem("Tools/Generate Scene Enum")]
		public static void Generate()
		{
			string enumName = "SceneType";
			string filePathAndName = "Assets/Scripts/GenerateCode/" + enumName + ".cs"; //The folder Scripts/Enums/ is expected to exist

			int sceneCount = SceneManager.sceneCountInBuildSettings;
			List<string> sceneNameList = new();
			for (int i = 0; i < sceneCount; i++)
			{
				sceneNameList.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
			}

			using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
			{
				streamWriter.WriteLine("using Helper;\n");

				streamWriter.WriteLine("namespace Personal.Constant");
				streamWriter.WriteLine("{");
				streamWriter.WriteLine("\tpublic enum " + enumName);
				streamWriter.WriteLine("\t{");

				for (int i = 0; i < sceneNameList.Count; i++)
				{
					string name = sceneNameList[i];
					streamWriter.WriteLine("\t\t[StringValue(\"" + name + "\")]");
					streamWriter.WriteLine("\t\t" + name + ",");

					if (i >= sceneNameList.Count - 1) break;
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