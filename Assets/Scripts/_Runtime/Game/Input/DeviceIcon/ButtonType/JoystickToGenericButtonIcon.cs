using Helper;

namespace Personal.InputProcessing
{
	/// <summary>
	/// This will not apply for every generic/third-party joystick controller because the buttons aren't standardized/bound towards the same path.
	/// Tested working on 8BitDo SN30pro+ (Joystick).
	/// </summary>
	public enum JoystickToGenericButtonIcon
	{
		[StringValue("Button1")] Button1 = 0,
		[StringValue("Button_South")] Button2,
		[StringValue("Button3")] Button3,
		[StringValue("Button_North")] Button4,
		[StringValue("Button_West")] Button5,
		[StringValue("Button6")] Button6,
		[StringValue("LeftShoulder")] Button7,
		[StringValue("RightShoulder")] Button8,
		[StringValue("LeftTrigger")] Button9,
		[StringValue("RightTrigger")] Button10,
		[StringValue("Button_Share")] Button11,
		[StringValue("Button_Option")] Button12,
		[StringValue("Button13")] Button13,
		[StringValue("LeftStickPress")] Button14,
		[StringValue("RightStickPress")] Button15,
		[StringValue("Button_East")] Trigger,
	}
}

