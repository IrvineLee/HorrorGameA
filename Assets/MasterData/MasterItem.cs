using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Helper;

[ExcelAsset(AssetPath = "MasterData/Data")]
public class MasterItem : ScriptableObject, ISerializationCallbackReceiver
{
	public List<EntityItem> EntityList;

	public IReadOnlyDictionary<int, EntityItem> Dictionary { get; private set; }

	public void OnBeforeSerialize() { }

	public void OnAfterDeserialize()
	{
		Dictionary = EntityList.ToDictionary(i => i.id);
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public EntityItem Get(int id)
	{
		var result = Dictionary.GetOrDefault(id);
		return result;
	}
}
