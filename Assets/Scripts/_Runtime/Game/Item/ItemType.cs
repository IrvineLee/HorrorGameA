
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

		[StringValue(AssetAddress.CubeBlue_Pickupable)]
		Item_3 = 1 << 2,

		[StringValue(AssetAddress.CubeBlue_Pickupable)]
		Item_4 = 1 << 3,

		All = Item_1 | Item_2 | Item_3 | Item_4,
	}
}