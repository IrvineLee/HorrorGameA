using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Personal.Constant;
using Personal.Data;
using Personal.Definition;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterSFX : MasterGeneric<SFXEntity>
{
#if UNITY_EDITOR
	/// <summary>
	/// Find the audio and set it to the definition.
	/// </summary>
	[ContextMenu("Refresh Definition")]
	void UpdateDefinition()
	{
		string definitionName = "SFX_Definition.asset";
		string path = ConstantFixed.DEFINITION_PATH + definitionName;

		// Create the asset.
		AssetDatabase.DeleteAsset(path);
		var audioDefinition = (SFXDefinition)CreateInstance(typeof(SFXDefinition));
		AssetDatabase.CreateAsset(audioDefinition, path);

		var definition = (SFXDefinition)AssetDatabase.LoadAssetAtPath(path, typeof(SFXDefinition));

		// Find and put value into the definition.
		foreach (var sfx in Entities)
		{
			string audioPath = ConstantFixed.SFX_PATH + sfx.name;
			AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
			definition.AudioList.Add(new SFXDefinition.Audio(sfx.audioSFXType, audioClip));
		}

		// Focus on the newly created asset.
		Selection.activeObject = definition;
		EditorGUIUtility.PingObject(Selection.activeObject);

		Debug.Log("Created " + definitionName + " successfully!");
	}
#endif
}
