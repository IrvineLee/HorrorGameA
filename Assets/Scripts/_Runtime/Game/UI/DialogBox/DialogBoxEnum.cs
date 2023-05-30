
using Helper;

namespace Personal.UI.Dialog
{
	public class DialogBoxEnum
	{
		/// <summary>
		/// The reason a dialog appears...
		/// </summary>
		public enum DialogUIType
		{
			DefaultButton = 0,
		}

		/// <summary>
		/// The type of dialog to display.
		/// </summary>
		public enum DialogDisplayType
		{
			[StringValue(AssetAddress.DialogButtonConfirmationBox)]
			DialogButtonConfirmationBox = 0,
		}

		/// <summary>
		/// The buttons attached to the display.
		/// </summary>
		public enum ButtonDisplayType
		{
			[StringValue(AssetAddress.Dialog_1Button_Ok)]
			One_Ok = 0,

			[StringValue(AssetAddress.Dialog_2Button_YesNo)]
			Two_YesNo,

			// These at the bottom are not done yet as of now.
			Three,
			Cancel,
		}
	}
}
