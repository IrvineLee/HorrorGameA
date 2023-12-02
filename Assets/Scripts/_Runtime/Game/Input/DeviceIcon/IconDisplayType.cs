
using Helper;

namespace Personal.InputProcessing
{
	public enum IconDisplayType
	{
		Auto = 0,

		[StringValue("KM_")]
		KeyboardMouse,

		[StringValue("DS4_")]
		Dualshock,

		[StringValue("XBox_")]
		Xbox,

		[StringValue("NS_")]
		NintendoSwitch,
	}
}

