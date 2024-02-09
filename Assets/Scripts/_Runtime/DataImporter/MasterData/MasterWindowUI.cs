
using Personal.Data;
using Personal.UI.Window;
using Helper;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterWindowUI : MasterGeneric<WindowUIEntity, int>
{
	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="windowUIType"></param>
	/// <returns></returns>
	public WindowUIEntity Get(WindowUIType windowUIType)
	{
		var result = Dictionary.GetOrDefault((int)windowUIType);
		return result;
	}
}
