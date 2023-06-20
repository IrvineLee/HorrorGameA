using System.Collections.Generic;

using Personal.Data;
using Helper;
using static Personal.UI.Window.WindowEnum;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterWindowUI : MasterGeneric<WindowUIEntity, WindowUIType>
{
	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<WindowUIType, WindowUIEntity>();

		// Somehow using Linq to change it to dictionary makes the comparing results not work.
		foreach (var entity in Entities)
		{
			dictionary.Add(entity.windowUIType, entity);
		}
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="windowUIType"></param>
	/// <returns></returns>
	public override WindowUIEntity Get(WindowUIType windowUIType)
	{
		var result = Dictionary.GetOrDefault(windowUIType);
		return result;
	}
}
