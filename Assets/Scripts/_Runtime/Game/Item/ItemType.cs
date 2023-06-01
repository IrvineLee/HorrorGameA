
using System;

namespace Personal.Item
{
	[Flags]
	public enum ItemType
	{
		Item_1 = 1 << 0,
		Item_2 = 1 << 1,
		Item_3 = 1 << 2,
		Item_4 = 1 << 3,

		All = Item_1 | Item_2 | Item_3 | Item_4,
	}
}