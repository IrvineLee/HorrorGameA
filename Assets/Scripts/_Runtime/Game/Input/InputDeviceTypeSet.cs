
using UnityEngine;

namespace Personal.InputProcessing
{
	public class InputDeviceTypeSet : MonoBehaviour
	{
		[SerializeField] InputDeviceType inputDeviceType = InputDeviceType.KeyboardMouse;

		public InputDeviceType InputDeviceType { get => inputDeviceType; }

		public string GetBindingGroup()
		{
			string str = "Keyboard";
			if (inputDeviceType == InputDeviceType.Gamepad) str = "Gamepad";
			else if (inputDeviceType == InputDeviceType.Joystick) str = "Joystick";

			return str;
		}
	}
}

