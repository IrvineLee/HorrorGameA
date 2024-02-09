
using Helper;

namespace Personal.UI.Window
{
	/// <summary>
	/// The buttons attached to the display.
	/// </summary>
	public enum WindowButtonDisplayType
	{
		[StringValue(AssetAddress.Window_1Button_Ok)]
		One_Ok = 0,

		[StringValue(AssetAddress.Window_2Button_YesNo)]
		Two_YesNo,

		// These at the bottom are not done yet as of now.
		Three,
		Cancel,
	}
}
