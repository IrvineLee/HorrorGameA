using Helper;

namespace Personal.FSM.Cashier
{
	public enum CashierItemType
	{
		[StringValue(AssetAddress.ItemParent_Day01_01)]
		ItemParent_Day01_01 = 0,

		[StringValue(AssetAddress.CashParent)]
		Cash = 10000
	}
}