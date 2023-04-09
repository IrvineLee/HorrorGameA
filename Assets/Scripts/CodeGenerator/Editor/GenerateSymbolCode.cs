using AltoLib;
using UnityEditor;

namespace Lizard.Editor
{
	public class GenerateSymbolCode
	{
		const string OutputDirPath = "Assets/Scripts/GenerateCode/";
		const string MyNamespace = "Personal";

		[MenuItem("Tools/Generate Code/Asset Address + Label")]
		static void GenerateAssetAddressCode()
		{
			Generate(new AssetAddressCodeGenerator());
			Generate(new AssetLabelCodeGenerator());
		}

		[MenuItem("Tools/Generate Code/Scene Name")]
		static void GenerateSceneNameCode()
		{
			Generate(new SceneNameCodeGenerator());
		}

		[MenuItem("Tools/Generate Code/Tag")]
		static void GenerateTagCode()
		{
			Generate(new TagCodeGenerator());
		}

		[MenuItem("Tools/Generate Code/Layer + Layer Mask")]
		static void GenerateLayerCode()
		{
			Generate(new LayerNameCodeGenerator());
			Generate(new LayerMaskCodeGenerator());
		}

		[MenuItem("Tools/Generate Code/All")]
		static void GenerateCode_All()
		{
			GenerateAssetAddressCode();
			GenerateSceneNameCode();
			GenerateTagCode();
			GenerateLayerCode();
		}

		static void Generate(CodeGenerator generator)
		{
			generator.outputDirPath = OutputDirPath;
			generator.namespaceName = MyNamespace;
			generator.Generate();
		}
	}
}