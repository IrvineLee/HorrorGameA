using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Setting.Control
{
	[Serializable]
	public class ControlData
	{
		// Map ui

		// Mouse and Keyboard controls
		[SerializeField] float mouseSensitivity = 1;

		// Gamepad controls
		[SerializeField] bool isVibration = true;

		public float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
		public bool IsVibration { get => isVibration; set => isVibration = value; }
	}
}