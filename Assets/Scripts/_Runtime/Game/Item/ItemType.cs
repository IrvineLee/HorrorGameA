
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
		Item_PuzzleBlock_A = 1 << 2,

		[StringValue(AssetAddress.PuzzleBlock_B_Pickupable)]
		Item_PuzzleBlock_B = 1 << 3,

		[StringValue(AssetAddress.Key_A_Pickupable)]
		Item_Key_A = 1 << 4,

		All = Item_1 | Item_2 | Item_PuzzleBlock_A | Item_PuzzleBlock_B,
	}
}