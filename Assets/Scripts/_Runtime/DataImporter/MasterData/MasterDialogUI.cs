using System.Collections.Generic;
using UnityEngine;

using Personal.Data;
using Helper;
using static MasterDialogUI;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterDialogUI : MasterGeneric<DialogUIEntity, DialogueUIType>
{
	public enum DialogueUIType
	{
		DefaultButton = 0,
	}

	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<DialogueUIType, DialogUIEntity>();

		// Somehow using Linq to change it to dictionary makes the comparing resutls not work.
		foreach (var entity in Entities)
		{
			dictionary.Add(entity.dialogueUIType, entity);
		}
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public override DialogUIEntity Get(DialogueUIType dialogueUIType)
	{
		var result = Dictionary.GetOrDefault(dialogueUIType);
		return result;
	}
}
