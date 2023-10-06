
using Helper;

namespace Personal.Item
{
	public enum ItemType
	{
		[StringValue(AssetAddress.CubeBlue_Pickupable)]
		Item_1 = 10000,

		[StringValue(AssetAddress.CubeRed_Pickupable)]
		Item_2 = 10001,

		[StringValue(AssetAddress.PuzzleBlock_A_Pickupable)]
		PuzzleBlock_A = 10100,

		[StringValue(AssetAddress.PuzzleBlock_B_Pickupable)]
		PuzzleBlock_B = 10101,

		[StringValue(AssetAddress.Key_A_Pickupable)]
		Key_A = 10201,

		[StringValue(AssetAddress.Book_A_Pickupable)]
		Book_A = 10301,
		Book_B = 10302,
		Book_C = 10303,
	}
}