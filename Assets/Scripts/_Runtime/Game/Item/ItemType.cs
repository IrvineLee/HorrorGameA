
using Helper;
using System;

namespace Personal.Item
{
	[Flags]
	public enum ItemType
	{
		[StringValue(AssetAddress.CubeBlue_Pickupable)]
		Item_1 = 1 << 0,

		[StringValue(AssetAddress.CubeRed_Pickupable)]
		Item_2 = 1 << 1,

		[StringValue(AssetAddress.PuzzleBlock_A_Pickupable)]
		PuzzleBlock_A = 1 << 2,

		[StringValue(AssetAddress.PuzzleBlock_B_Pickupable)]
		PuzzleBlock_B = 1 << 3,

		[StringValue(AssetAddress.Key_A_Pickupable)]
		Key_A = 1 << 4,

		[StringValue(AssetAddress.Book_A_Pickupable)]
		Book_A = 1 << 5,
		Book_B = 1 << 6,
		Book_C = 1 << 7,

		All = Item_1 | Item_2 | PuzzleBlock_A | PuzzleBlock_B | Book_A | Book_B | Book_C,
	}
}