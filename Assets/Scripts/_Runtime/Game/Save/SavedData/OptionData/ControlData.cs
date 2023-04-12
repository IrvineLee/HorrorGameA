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

		// Controls
		[SerializeField] bool isVibration = true;


		public bool IsVibration { get => isVibration; set => isVibration = value; }
	}
}