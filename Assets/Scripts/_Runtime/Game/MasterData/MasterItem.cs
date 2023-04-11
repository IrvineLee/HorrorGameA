using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Helper;

[ExcelAsset(AssetPath = "MasterData/Data")]
public class MasterItem : ScriptableObject, ISerializationCallbackReceiver
{
	public List<ItemEntity> ItemList;

	public IReadOnlyDictionary<int, ItemEntity> Dictionary { get; private set; }

	public void OnBeforeSerialize() { }

	public void OnAfterDeserialize()
	{
		Dictionary = ItemList.ToDictionary(i => i.id);
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public ItemEntity Get(int id)
	{
		var result = Dictionary.GetOrDefault(id);
		return result;
	}
}
