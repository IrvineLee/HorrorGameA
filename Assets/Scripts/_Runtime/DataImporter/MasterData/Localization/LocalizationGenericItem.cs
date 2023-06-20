using System.Collections.Generic;

using Personal.Data;
using Personal.Item;
using Helper;

[ExcelAsset(AssetPath = "Data/MasterData/Localization")]
public class LocalizationGenericItem : MasterGeneric<LocalizationItemEntity, ItemType>
{
	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<ItemType, LocalizationItemEntity>();

		// Somehow using Linq to change it to dictionary makes the comparing results not work.
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
	public override LocalizationItemEntity Get(ItemType itemType)
	{
		var result = Dictionary.GetOrDefault(itemType);
		return result;
	}
}
