
using Helper;

namespace Personal.UI.Window
{
	public class WindowEnum
	{
		/// <summary>
		/// The reason a window appears...
		/// </summary>
		public enum WindowUIType
		{
			DefaultButton = 0,
		}

		/// <summary>
		/// The type of window to display.
		/// </summary>
		public enum WindowDisplayType
		{
			[StringValue(AssetAddress.WindowButtonConfirmationBox)]
			ButtonConfirmationBox = 0,
		}

		/// <summary>
		/// The buttons attached to the display.
		/// </summary>
		public enum ButtonDisplayType
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
}
