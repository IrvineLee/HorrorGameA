using System.Collections.Generic;

using Personal.Data;
using Personal.Item;
using Helper;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterItem : MasterGeneric<ItemEntity, ItemType>
{
	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<ItemType, ItemEntity>();

		foreach (var entity in Entities)
		{
			dictionary.Add(entity.itemType, entity);
		}
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="itemType"></param>
	/// <returns></returns>
	public override ItemEntity Get(ItemType itemType)
	{
		var result = Dictionary.GetOrDefault(itemType);
		return result;
	}
}
