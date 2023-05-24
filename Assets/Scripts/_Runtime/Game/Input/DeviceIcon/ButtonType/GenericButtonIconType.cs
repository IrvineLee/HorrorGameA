
using Helper;

namespace Personal.InputProcessing
{
	public enum GenericButtonIconType
	{
		[StringValue("Dpad")] Dpad = 0,                             // Dpad
		[StringValue("Dpad_Up")] Dpad_Up,
		[StringValue("Dpad_Down")] Dpad_Down,
		[StringValue("Dpad_Left")] Dpad_Left,
		[StringValue("Dpad_Right")] Dpad_Right,
		[StringValue("Dpad_UpDown")] Dpad_UpDown,
		[StringValue("Dpad_LeftRight")] Dpad_LeftRight,

		[StringValue("LeftStick")] LeftStick,                       // Left analog stick
		[StringValue("LeftStick_Up")] LeftStick_Up,
		[StringValue("LeftStick_Down")] LeftStick_Down,
		[StringValue("LeftStick_Left")] LeftStick_Left,
		[StringValue("LeftStick_Right")] LeftStick_Right,
		[StringValue("LeftStick_UpDown")] LeftStick_UpDown,
		[StringValue("LeftStick_LeftRight")] LeftStick_LeftRight,

		[StringValue("RightStick")] RightStick,                     // Right analog stick
		[StringValue("RightStick_Up")] RightStick_Up,
		[StringValue("RightStick_Down")] RightStick_Down,
		[StringValue("DpaRightStick_Leftd")] RightStick_Left,
		[StringValue("RightStick_Right")] RightStick_Right,
		[StringValue("RightStick_UpDown")] RightStick_UpDown,
		[StringValue("RightStick_LeftRight")] RightStick_LeftRight,

		[StringValue("Button_North")] Button_North,                 // Triangle
		[StringValue("Button_South")] Button_South,                 // Cross
		[StringValue("Button_East")] Button_East,                   // Circle
		[StringValue("Button_West")] Button_West,                   // Square

		[StringValue("LeftBumper")] LeftBumper,                     // L1
		[StringValue("LeftTrigger")] LeftTrigger,                   // L2
		[StringValue("LeftStickPress")] LeftStickPress,             // L3

		[StringValue("RightBumper")] RightBumper,                   // R1
		[StringValue("RightTrigger")] RightTrigger,                 // R2
		[StringValue("RightStickPress")] RightStickPress,           // R3

		[StringValue("Button_Option")] Button_Option,
		[StringValue("Button_Share")] Button_Share,
		[StringValue("Button_Home")] Button_Home,
		[StringValue("Touchpad")] Touchpad,
	}
}

