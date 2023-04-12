using UnityEngine;

using Personal.Data;
using Personal.Definition;
using Personal.Constant;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterAudio : MasterGeneric<BGMEntity>
{
#if UNITY_EDITOR
	/// <summary>
	/// Find the audio and set it to the definition.
	/// </summary>
	[ContextMenu("Refresh Definition")]
	void UpdateDefinition()
	{
		string definitionName = "BGM_Definition.asset";
		string path = ConstantFixed.DEFINITION_PATH + definitionName;

		// Create the asset.
		AssetDatabase.DeleteAsset(path);
		var audioDefinition = (BGMDefinition)CreateInstance(typeof(BGMDefinition));
		AssetDatabase.CreateAsset(audioDefinition, path);

		var definition = (BGMDefinition)AssetDatabase.LoadAssetAtPath(path, typeof(BGMDefinition));

		// Find and put value into the definition.
		foreach (var bgm in Entities)
		{
			string audioPath = ConstantFixed.BGM_PATH + bgm.name;
			AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
			definition.AudioList.Add(new BGMDefinition.Audio(bgm.audioBGMType, audioClip));
		}

		// Focus on the newly created asset.
		Selection.activeObject = definition;
		EditorGUIUtility.PingObject(Selection.activeObject);

		Debug.Log("Created " + definitionName + " successfully!");
	}
#endif
}
