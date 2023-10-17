
using Helper;

namespace Personal.Item
{
	// You should add the string value so it can be instantiated in the inventory.
	public enum ItemType
	{
		[StringValue(AssetAddress.CubeBlue_Pickupable)]
		_10000_Item_1 = 10000,

		[StringValue(AssetAddress.CubeRed_Pickupable)]
		_10001_Item_2 = 10001,

		[StringValue(AssetAddress.PuzzleBlock_A_Pickupable)]
		_10100_PuzzleBlock_A = 10100,

		[StringValue(AssetAddress.PuzzleBlock_B_Pickupable)]
		_10101_PuzzleBlock_B = 10101,

		[StringValue(AssetAddress.Key_A_Pickupable)]
		_10201_Key_A = 10201,

		[StringValue(AssetAddress.Book_A_Pickupable)]
		_10301_Book_A = 10301,
		_10302_Book_B = 10302,
		_10303_Book_C = 10303,
	}
}