using System.Collections.Generic;

using Personal.Data;
using Helper;
using static Personal.UI.Dialog.DialogBoxEnum;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterDialogUI : MasterGeneric<DialogUIEntity, DialogUIType>
{
	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<DialogUIType, DialogUIEntity>();

		// Somehow using Linq to change it to dictionary makes the comparing resutls not work.
		foreach (var entity in Entities)
		{
			dictionary.Add(entity.dialogUIType, entity);
		}
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="dialogUIType"></param>
	/// <returns></returns>
	public override DialogUIEntity Get(DialogUIType dialogUIType)
	{
		var result = Dictionary.GetOrDefault(dialogUIType);
		return result;
	}
}
