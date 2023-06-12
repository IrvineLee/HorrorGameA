
using Personal.Constant;
using Personal.Data;
using Personal.Definition;
using UnityEngine;

using UnityEditor;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterBGM : MasterGeneric<BGMEntity, int>
{
#if UNITY_EDITOR

	/// <summary>
	/// Find the audio and set it to the definition.
	/// </summary>
	[ContextMenu("Refresh Definition")]
	void UpdateDefinition()
	{
		string definitionName = "BGM_Definition.asset";
		string path = ConstantFixed.DEFINITION_AUDIO_PATH + definitionName;

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

		// Make sure the data is saved correctly.
		EditorUtility.SetDirty(definition);

		Debug.Log("Created " + definitionName + " successfully!");
	}
#endif
}
